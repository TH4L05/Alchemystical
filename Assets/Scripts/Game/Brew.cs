
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;
using TK.Audio;

namespace Alchemystical
{
    public class Brew : MonoBehaviour
    {
        #region Events

        public static Action<Potion> PotionIsBrewed;
        public static Action<int,EffectType> EffectAdded;
        public static Action<Ingredient> IngredientAdded;
        public static Action ResetBrewInfo;
        public static Action<bool> ChangeBrewInfoStatus;
        public static Action<string> ShowInfoText;

        public UnityEvent OnBrewModeActivate;
        public UnityEvent OnBrewModeDeactivate;

        #endregion

        #region SerializedFields

        [Header("Laddle")]
        [SerializeField] private GameObject laddle;
        [SerializeField] private Transform laddleStart;
        [SerializeField] private Vector3 laddleStartRotation = Vector3.zero;

        [Header("Visual")]
        [SerializeField] private GameObject brewLiquid;
        [SerializeField] private ParticleSystem bubbleParticle;
        [SerializeField] private ParticleSystem smokeParticle;
        [SerializeField] private Color defaultBrewLiquidColor = Color.white;
        [SerializeField] private TextMeshProUGUI statusButtonTextField;

        [Header("Settings")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private float maxDistance = 1f;
        [SerializeField] private float splittingSize = 45;

        [Header("Settings")]
        [SerializeField] private AudioEventList audioEventList;

        #endregion
       
        public static bool onBrewMode;

        #region PrivateFields

        private bool isActive;
        private bool activated;

        private Vector2 inputVector;
        private int currentDirection = 0;
        private Vector3 laddleIdlePostion;
        private Vector3 laddleIdleRotation;
        private Vector3 currentLaddlePosition;
        private float distance;
        private float x;
        private float y;
        private Vector3 p;
        private float angle;
        private int lastSplit = 0;
               
        private Material brewLiquidMaterial => brewLiquid.GetComponent<MeshRenderer>().material;
        private Ingredient[] ingredients;
        private Ingredient currentIngredient;
        private Potion potion;
        private bool canAddIngredient;
        private int effectCounter;

        //public int clockWiseFinished;
        //public int counterClockWiseFinished;

        #endregion
    
        #region UnityFunctions

        private void Awake()
        {
            laddleIdlePostion = laddle.transform.position;
            laddleIdleRotation = laddle.transform.rotation.eulerAngles;
        }

        private void Start()
        {
            ingredients = Game.Instance.gameData.ingredients;
            InventoryUISlot.AddIngredientFromSlot += AddIngredient;
            GameInput.ToggleBrewMode += ToggleBrewMode;
            GameInput.MixInputValuesChanged += UpdateInputVector;
        }

        private void OnDestroy()
        {
            InventoryUISlot.AddIngredientFromSlot -= AddIngredient;
            GameInput.ToggleBrewMode -= ToggleBrewMode;
            GameInput.MixInputValuesChanged -= UpdateInputVector;
        }

        private void Update()
        {
            BrewMovement();
            Direction();          
        }

        #endregion

        #region BrewStatus

        public void ToggleBrewMode()
        {
            onBrewMode = !onBrewMode;
            audioEventList.PlayAudioEventOneShot("ButtonClick");
            ChangeBrewMode(onBrewMode);
        }

        private void ChangeBrewMode(bool active)
        {
            if (laddle == null) throw new ArgumentNullException("Laddle Object Ref is Missing");
            if (brewLiquid == null) throw new ArgumentNullException("BrewLiquid Object Ref is Missing");


            if (active)
            {
                if (activated) return;

                activated = active;

                effectCounter = 0;
                //clockWiseFinished = 0;
                //counterClockWiseFinished = 0;
                laddle.transform.position = laddleStart.position;
                laddle.transform.rotation = Quaternion.Euler(laddleStartRotation);
                NewPotion();
                ResetBrewColors();
                canAddIngredient = true;

                if (statusButtonTextField != null) statusButtonTextField.text = "Deactivate Brewmode";
                ChangeBrewInfoStatus?.Invoke(active);
                OnBrewModeActivate?.Invoke();

                Game.Instance.input.SetBrewInputActionsStatus(true);
                Game.Instance.inventory.ChangeInvewntoryUIObjectStatus(true);
                Game.Instance.inventory.ChangeInventoryUISlotButtonInteractability(true);
            }
            else
            {
                if (!activated) return;

                activated = active;

                Game.Instance.input.SetBrewInputActionsStatus(false);
                Game.Instance.inventory.ChangeInventoryUISlotButtonInteractability(false);
                Game.Instance.inventory.ChangeInvewntoryUIObjectStatus(false);

                canAddIngredient = false;
                effectCounter = 0;
                //clockWiseFinished = 0;
                //counterClockWiseFinished = 0;
                laddle.transform.position = laddleIdlePostion;
                laddle.transform.rotation = Quaternion.Euler(laddleIdleRotation);
                DestroyPotion();
                ResetBrewColors();
                ChangeBrewInfoStatus?.Invoke(active);
                OnBrewModeDeactivate?.Invoke();

                if (statusButtonTextField != null) statusButtonTextField.text = "Activate Brewmode";
            }
        }

        #endregion

        #region Brew

        private static float Angle(Vector3 vec)
        {
            if (vec.x < 0)
            {
                return 360 - (Mathf.Atan2(vec.x, vec.z) * Mathf.Rad2Deg * -1);
            }
            else
            {
                return Mathf.Atan2(vec.x, vec.z) * Mathf.Rad2Deg;
            }
        }

        private void UpdateInputVector(Vector2 input)
        {
            inputVector = input;
        }

        private void BrewMovement()
        {
            if (!onBrewMode) return;

            
            if (inputVector != Vector2.zero)
            {
                x += inputVector.y * speed * Time.deltaTime;
                y += -inputVector.x * speed * Time.deltaTime;

                x = Mathf.Clamp(x, -maxDistance, maxDistance);
                y = Mathf.Clamp(y, -maxDistance, maxDistance);

                p = new Vector3(laddleStart.position.x + x, laddleStart.position.y, laddleStart.position.z + y);
                laddle.transform.position = p;
                currentLaddlePosition = laddle.transform.position;
            }
            else
            {
                x = 0;
                y = 0;
                laddle.transform.position = laddleStart.position;
                lastSplit = 0;
                angle = 0;
                //clockWiseFinished = 0;
                //counterClockWiseFinished = 0;
            }
        }

        private void Direction()
        {
            if (inputVector == Vector2.zero)
            {
                currentDirection = 0;
                return;
            }

            angle = Angle(laddle.transform.position - laddleStart.position);

            for (int i = 0; i < Mathf.RoundToInt(360 / splittingSize); i++)
            {
                if (angle > i * splittingSize && angle < (i + 1) * (splittingSize))
                {
                    if (i == lastSplit)
                    {
                        break;
                    }
                    else
                    {
                        if (i == (lastSplit + 1) % 8 || ((lastSplit == 7) && (i == 1)))
                        {
                            if (currentDirection > 0)
                            {
                                currentDirection++;
                            }

                            if (currentDirection < 0)
                            {
                                currentDirection = 1;
                            }

                            if (currentDirection == 0)
                            {
                                currentDirection = 1;
                            }

                        }
                        else if (i == (lastSplit - 1) % 8 || ((lastSplit == 0) && (i == 7)))
                        {
                            if (currentDirection < 0)
                            {
                                currentDirection--;
                            }

                            if (currentDirection > 0)
                            {
                                currentDirection = -1;
                            }

                            if (currentDirection == 0)
                            {
                                currentDirection = -1;
                            }
                        }
                        else
                        {
                            currentDirection = 0;
                        }
                        lastSplit = i;
                    }
                }
            }

            if (currentDirection > 8)
            {
                currentDirection = 0;
                //lastSplit = 0;
                //angle = 0;

                Debug.Log("Clockwise");
                if (currentIngredient == null) return;
                AddEffectToPotion(currentIngredient, MotionType.Clockwise);
            }

            if (currentDirection < -8)
            {
                currentDirection = 0;
                //lastSplit = 0;
                //angle = 0;

                Debug.Log("CounterClockwise");
                if (currentIngredient == null) return;
                AddEffectToPotion(currentIngredient, MotionType.Counterclockwise);              
            }
        }

        private void ResetBrewColors()
        {
            brewLiquidMaterial.color = defaultBrewLiquidColor;
            ParticleSystem.MainModule bubbleMain = bubbleParticle.main;
            bubbleMain.startColor = defaultBrewLiquidColor;
            ParticleSystem.MainModule smokeMain = smokeParticle.main;
            smokeMain.startColor = defaultBrewLiquidColor;
        }

        private void SetBrewColors(Color color)
        {
            ParticleSystem.MainModule smokeMain = smokeParticle.main;
            ParticleSystem.MainModule bubbleMain = bubbleParticle.main;
            

            if (effectCounter == 1)
            {
                brewLiquidMaterial.color = color;
                bubbleMain.startColor = color;
                smokeMain.startColor = color;
            }
            var finalColor = (brewLiquidMaterial.color + color) / 2;
            //var finalColor = Color.Lerp(brewLiquidMaterial.color, color, 1f);
            brewLiquidMaterial.color = finalColor;
            bubbleMain.startColor = finalColor;
            smokeMain.startColor = finalColor;
        }

        #endregion

        #region Potion

        private void NewPotion()
        {
            potion = ScriptableObject.Instantiate(new Potion());
            potion.name = "UnknownPotion";
            potion.potionName = "UnknownPotion";
            potion.lockedEffects = false;
            ResetBrewInfo?.Invoke();
        }

        public void ResetPotion()
        {
            DestroyPotion();
            NewPotion();
        }

        private void DestroyPotion()
        {
            if (potion != null) DestroyImmediate(potion);
            potion = null;
            ResetBrewColors();
            ResetBrewInfo?.Invoke();
            canAddIngredient = true;
            effectCounter = 0;
        }

        private void AddEffectToPotion(Ingredient ingredient, MotionType motionType)
        {
            Debug.Log("AddEffect");
            if (effectCounter >= 3) return;
            EffectType effectType = EffectType.Nothing;

            if (potion == null) return;
            Debug.Log("AddEffect");
            switch (motionType)
            {
                case MotionType.Invalid:
                    break;

                case MotionType.Clockwise:
                    effectType = ingredient.clockwiseEffect;
                    potion.AddEffect(effectType);

                    if (!ingredient.cwEffectUnlocked)
                    {
                        ingredient.cwEffectUnlocked = true;
                    }
                    
                    break;
                case MotionType.Counterclockwise:
                    effectType = ingredient.counterClockwiseEffect;
                    potion.AddEffect(effectType);

                    if (!ingredient.ccwEffectUnlocked)
                    {
                        ingredient.ccwEffectUnlocked = true;
                    }
                                     
                    break;

                case MotionType.Count:
                    break;

                default:
                    break;
            }

            //if (effectType == EffectType.Nothing) return;
            effectCounter++;

            audioEventList.PlayAudioEventOneShot("PotionAddEffect");
            string text = "Add Effect ->" + effectType.ToString();
            ShowInfoText?.Invoke(text);
            EffectAdded?.Invoke(effectCounter, effectType);
                     
            canAddIngredient = true;
            currentIngredient = null;
                         
            if (effectCounter >= 3)
            {
                canAddIngredient = false;
                PotionFinished();
            }
        }

        private void PotionFinished()
        {
            CheckPotion(potion);
        }

        private void CheckPotion(Potion brewedPotion)
        {
            Potion[] gamePotions = Game.Instance.gameData.potions;
            bool[] effectSucces = new bool[3];
            bool success = false;

            foreach (var potion in gamePotions)
            {
                success = false;
                List<EffectType> potionEffects = potion.effects;
                bool[] effectSuccess = new bool[3];

                for (int i = 0; i < potionEffects.Count; i++)
                {
                    for (int p = 0; p < brewedPotion.effects.Count; p++)
                    {
                        if (brewedPotion.effects[i] == potionEffects[p])
                        {
                            effectSuccess[i] = true; 
                            break;
                        }
                    }
                }

                success = CheckSucces(effectSuccess);
                if (success)
                {
                    brewedPotion.potionName = potion.potionName;
                    PotionFinshSetup(brewedPotion);
                    return;
                }
            }
            PotionFinshSetup(brewedPotion);

        }

        public bool CheckSucces(bool[] effectsSuccess)
        {
            for (int i = 0; i < effectsSuccess.Length; i++)
            {
                if (effectsSuccess[i] == false)
                {
                    return false;
                }
            }
            return true;
        }

        private void PotionFinshSetup(Potion potion)
        {

            string text = potion.potionName;
            ShowInfoText?.Invoke(text);

            if (potion.potionName == "UnknownPotion")
            {
                ResetPotion();
                return;
            }

            var finishedPotion = ScriptableObject.Instantiate(potion);
            finishedPotion.name = potion.potionName;
            finishedPotion.potionName = potion.potionName;
            finishedPotion.potionPicture = potion.potionPicture;

            PotionIsBrewed?.Invoke(finishedPotion);
            audioEventList.PlayAudioEventOneShot("PotionBrewed");          
            ResetPotion();
        }

        #endregion

        #region Ingredient

        private void AddIngredient(Ingredient ingredient)
        {
            if (!activated) return;
            if (effectCounter >= 3) return;
            if (!canAddIngredient) return;
            if (ingredient == null) return; 

            canAddIngredient = false;
            currentIngredient = ingredient;
            SetBrewColors(currentIngredient.brewColor);
            Game.Instance.inventory.RemoveItemAmount(currentIngredient, 1);
            audioEventList.PlayAudioEventOneShot("AddIngredient");
            IngredientAdded?.Invoke(currentIngredient);
        }

        #endregion
    }
}


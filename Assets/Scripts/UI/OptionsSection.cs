using Alchemystical;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alchemystical
{
    public class OptionsSection : MonoBehaviour
    {
        [SerializeField] private List<OptionsSubsection> optionsSubsections = new List<OptionsSubsection>();


        public void DeactivateAllSubsections()
        {
            foreach (var section in optionsSubsections)
            {
                section.Deactivate();
            }
        }
    }
}


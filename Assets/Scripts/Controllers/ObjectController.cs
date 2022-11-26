using System.Collections.Generic;
using System.Linq;
using Configs;
using UnityEngine;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.MeasureResource;

namespace Controllers
{
    public class ObjectController : MonoBehaviour
    {
        private static readonly string FilePath = Config.ConfigGo.PublicObjectPath;
        private readonly IfcStore _model = IfcStore.Open(FilePath);
    
        private readonly List<string> _idStringList = new();
        private readonly List<GameObject> _idGoList = new();

        private void OnEnable()
        {
            if (_model == null)
                Debug.LogError("This IFC file is not available, make sure you open the right path!");
        }

        /// <summary>
        /// Imports all the metadata of an .ifc file and adds all the id's from a specific property of the whole model.
        /// </summary>
        public void LoadModel()
        {
            var ifcObjects = _model.Instances.OfType<IIfcColumn>();

            foreach (var ifcObject in ifcObjects)
                _idStringList.Add(ifcObject.GlobalId);


            foreach (var goWithIDs in _idStringList.Select(ids => GameObject.Find(ids)).Where(goWithIDs => !_idGoList.Contains(goWithIDs))) 
                _idGoList.Add(goWithIDs);
        }
        
    
        /// <summary>
        /// Toggles all the GameObjects in the list enabled or disabled. Depending on if it's already enabled or not.
        /// </summary>
        private bool _goIsEnabled = true;
        
        public void ToggleGOsInList()
        {
            if (_goIsEnabled == false)
            {
                foreach (var enableGo in _idGoList)
                    enableGo.SetActive(true);

                _goIsEnabled = true;
            }
            else
            {
                foreach (var disableGo in _idGoList)
                    disableGo.SetActive(false);

                _goIsEnabled = false;
            }
        }

    
        /// <summary>
        /// Filters on a property of the model. So in this case it filters and adds the objects to a list if the length of the object is higher then 2000.
        /// </summary>
        private readonly List<string> _filteredIDStringList = new();
        private readonly List<GameObject> _filteredIdGoList = new();
        public void FilterProperty()
        {
            var filterModelObjects = _model.Instances.Where<IIfcColumn>(LengthColumnFilter);
            
            bool LengthColumnFilter(IIfcColumn modelObject)
            {
                return modelObject.IsDefinedBy
                    .Where(related => related.RelatingPropertyDefinition is IIfcPropertySet)
                    .SelectMany(related => ((IIfcPropertySet) related.RelatingPropertyDefinition).HasProperties)
                    .OfType<IIfcPropertySingleValue>()
                    .Any(property => property.Name == "Length" && property.NominalValue is IfcLengthMeasure length && length > 2000);
            }

            foreach (var modelObject in filterModelObjects)
                _filteredIDStringList.Add(modelObject.GlobalId);

            foreach (var modelGo in _filteredIDStringList.Select(ids => GameObject.Find(ids)).Where(modelGo => !_filteredIdGoList.Contains(modelGo)))
                _filteredIdGoList.Add(modelGo);
        }

        /// <summary>
        /// Toggles all the filtered GameObjects in the list enabled or disabled. Depending on if it's already enabled or not.
        /// </summary>
        private bool _filteredGoIsEnabled = true;
    
        public void ToggleFilteredGOsInList()
        {
            if (_filteredGoIsEnabled == false)
            {
                foreach (var enableGo in _filteredIdGoList)
                    enableGo.SetActive(true);

                _filteredGoIsEnabled = true;
            }
            else
            {
                foreach (var disableGo in _filteredIdGoList)
                    disableGo.SetActive(false);

                _filteredGoIsEnabled = false;
            }
        }
    }
}
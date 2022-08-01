using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BilliardCruise.Sava.Scripts
{
    [CreateAssetMenu(fileName = "TablesList", menuName = "Tables/TablesList", order = 1)]
    public class TablesList : ScriptableObject
    {

        [SerializeField] private string collectionName = "";
        [SerializeField] private List<TableProperties> tables;

        public string CollectionName
        {
            get
            {
                return collectionName;
            }
        }

        public List<TableProperties> Tables
        {
            get
            {
                return tables;
            }
        }

        public TableProperties GetTable(string tableId)
        {


            foreach (TableProperties table in Tables)
            {
                if (table.TableId.Equals(tableId))
                {
                    return table;
                }
            }

            return null;
        }

    }
}
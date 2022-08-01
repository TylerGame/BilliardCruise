
using UnityEngine;


namespace BilliardCruise.Sava.Scripts
{

    [CreateAssetMenu(fileName = "TableProperties", menuName = "Tables/TablePropertiesObject", order = 1)]
    public class TableProperties : ScriptableObject
    {

        [SerializeField] private string tableId = "";
        [SerializeField] private string tableName = "";
        [SerializeField] private Sprite tableSprite;
        [SerializeField] private Sprite thumbnailSprite;

        [SerializeField] private float price = 0;

        public string TableId
        {
            get
            {
                return tableId;
            }
        }

        public string TableName
        {
            get
            {
                return tableName;
            }
        }

        public Sprite TableSprite
        {
            get
            {
                return tableSprite;
            }
        }

        public Sprite ThumbnailSprite
        {
            get
            {
                return thumbnailSprite;
            }
        }

        public float Price
        {
            get
            {
                return price;
            }
        }

    }
}

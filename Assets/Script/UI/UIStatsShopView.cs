using UnityEngine;

namespace Script.UI
{
    public class UIStatsShopView : MonoBehaviour
    {
        public void OnRefresh(int coinAmount)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                UIShopStatsElement element = transform.GetChild(i).GetComponent<UIShopStatsElement>();
                element.SetElement(coinAmount);
            }
        }
    }
}
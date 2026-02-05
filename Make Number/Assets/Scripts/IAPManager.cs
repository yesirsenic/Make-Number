using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    public static IAPManager Instance;

    private const string PRODUCT_NO_ADS = "no_ads";

    private StoreController store;
    private bool initialized;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        await Initialize();
    }

    private async Task Initialize()
    {
        store = UnityIAPServices.StoreController();

        store.OnPurchasePending += OnPurchasePending;
        store.OnProductsFetched += OnProductsFetched;
        store.OnPurchasesFetched += OnPurchasesFetched;

        await store.Connect();

        store.FetchProducts(new List<ProductDefinition>
        {
            new ProductDefinition(PRODUCT_NO_ADS,
            UnityEngine.Purchasing.ProductType.NonConsumable)
        });
    }

    private void OnProductsFetched(List<UnityEngine.Purchasing.Product> products)
    {
        initialized = true;
        store.FetchPurchases();
    }

    private void OnPurchasesFetched(Orders orders)
    {
        foreach (var order in orders.ConfirmedOrders)
        {
            if (OrderContains(order, PRODUCT_NO_ADS))
            {
                NoAdsManager.Instance.SetNoAdsPurchased();
                return;
            }
        }
    }

    private void OnPurchasePending(PendingOrder pending)
    {
        if (OrderContains(pending, PRODUCT_NO_ADS))
        {
            NoAdsManager.Instance.SetNoAdsPurchased();
        }

        store.ConfirmPurchase(pending);
    }

    public void BuyNoAds()
    {
        if (!initialized)
        {
            Debug.LogWarning("[IAP] Not initialized");
            return;
        }

        if (NoAdsManager.Instance.HasNoAds)
            return;

        store.PurchaseProduct(PRODUCT_NO_ADS);
    }

    private bool OrderContains(Order order, string productId)
    {
        var list = order.Info?.PurchasedProductInfo;
        if (list == null) return false;

        foreach (var info in list)
        {
            if (info.productId == productId)
                return true;
        }

        return false;
    }
}

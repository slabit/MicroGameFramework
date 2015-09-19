#if UseGKExtensions
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.Cloud.Analytics;

using gk;

//namespace gk
//{
[AddComponentMenu("GK/InAppPurchase/gkInAppPurchaseManager")]
public class gkInAppPurchaseManager : MonoBehaviour
{	
	public List<gkInAppPurchaseProductID> products = new List<gkInAppPurchaseProductID>();
	
	public string androidPublicKey;
	
	private Dictionary<string, gkInAppPurchaseProductID> m_oProductById = new Dictionary<string, gkInAppPurchaseProductID>();
	
	private Action<bool> m_rOnPurchaseResult;
	
	private bool m_bInAppAvailable = false;
	
	private bool m_bPurchasingInProgress;

	#if freePurchase 
	private static bool ms_bDebugFreePurchase = true;
	#else
	private static bool ms_bDebugFreePurchase = false;
	#endif

	#if UNITY_IPHONE
	Dictionary<string, StoreKitProduct> storeKitProductByID = new Dictionary<string, StoreKitProduct>();
	#endif
	
	#if UNITY_ANDROID
	Dictionary<string, GoogleSkuInfo> skuInfosByID = new Dictionary<string, GoogleSkuInfo>();
	HashSet<string> consummables = new HashSet<string>();
	string productTryingToPurchase = "";
	#endif

	static private gkInAppPurchaseManager ms_oInstance;
	
	static public gkInAppPurchaseManager Instance
	{
		get
		{
			return ms_oInstance;
		}
	}
	
	public void TryPurchase(string a_oProductID, Action<bool> a_rOnPurchaseResult)
	{
		if(m_bInAppAvailable == false && ms_bDebugFreePurchase == false)
		{
			Debug.Log("Try Purchase but in app purchase is not available");
			gkNativePopUpManager.Instance.ShowPopUp("", "In app purchase unavailable");
			return;
		}
		
		if(m_bPurchasingInProgress)
		{
			Debug.Log("Purchasing in progress");
			return;
		}
		m_bPurchasingInProgress = true;
		
		m_rOnPurchaseResult = a_rOnPurchaseResult;
		
		gkInAppPurchaseProductID oProduct;
		if(m_oProductById.TryGetValue(a_oProductID, out oProduct))
		{
			gkNativeActivityUtility.ShowActivityView(true);
			LaunchPurchase(oProduct);
		}
		else
		{
			OnPurchaseResult(false);
		}
	}
	
	private void Awake()
	{
		//Debug.Log(androidPublicKey);
		if(ms_oInstance == null)
		{
			ms_oInstance = this;
		}
		else
		{
			Debug.LogWarning("A singleton can only be instantiated once!");
			Destroy(gameObject);
			return;
		}
		
		FillProductById();
		Initialize();
	}
	
	private void OnDestroy()
	{
		Terminate();
	}
	
	private void Initialize()
	{
		Debug.Log("Initialize in app purchase");
		
		m_bInAppAvailable = false;
		
		#if UNITY_EDITOR
		m_bInAppAvailable = true;
		#elif UNITY_IPHONE
		StoreKitManager.purchaseSuccessfulEvent += IOSPurchaseSuccessful;
		StoreKitManager.purchaseFailedEvent += IOSPurchaseFailed;
		StoreKitManager.purchaseCancelledEvent += IOSPurchaseCancel;
		
		StoreKitManager.productListReceivedEvent += IOSProductsReceived;
		StoreKitManager.productListRequestFailedEvent += IOSProductsRequestFailed;
		
		// Request product
		string[] oProductIdentifiers = new string[products.Count];
		for(int i = 0; i < products.Count; ++i)
		{
			oProductIdentifiers[i] = products[i].iosProductID;
		}
		StoreKitBinding.requestProductData(oProductIdentifiers);
		#elif UNITY_ANDROID
		GoogleIABManager.purchaseSucceededEvent += AndroidPurchaseSuccedeed;
		GoogleIABManager.purchaseFailedEvent += AndroidPurchaseFailed;
		
		GoogleIABManager.billingSupportedEvent += AndroidBillingSupported;
		GoogleIABManager.billingNotSupportedEvent += AndroidBillingNotSupported;
		
		GoogleIABManager.queryInventorySucceededEvent += AndroidQueryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += AndroidQueryInventoryFailedEvent;

		GoogleIABManager.consumePurchaseFailedEvent += ConsumePurchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent += ConsumePurchaseSucceededEvent;

		foreach(gkInAppPurchaseProductID product in products)
		{
			if(product.consumable)
			{
				consummables.Add(product.androidProductID);
			}
		}

		GoogleIAB.init(androidPublicKey);
		#else
		m_bInAppAvailable = true;
		#endif
	}
	
	private void Terminate()
	{
		#if UNITY_EDITOR
		#elif UNITY_IPHONE
		StoreKitManager.purchaseSuccessfulEvent -= IOSPurchaseSuccessful;
		StoreKitManager.purchaseFailedEvent -= IOSPurchaseFailed;
		StoreKitManager.purchaseCancelledEvent -= IOSPurchaseCancel;
		
		StoreKitManager.productListReceivedEvent -= IOSProductsReceived;
		StoreKitManager.productListRequestFailedEvent -= IOSProductsRequestFailed;
		#elif UNITY_ANDROID
		GoogleIABManager.purchaseSucceededEvent -= AndroidPurchaseSuccedeed;
		GoogleIABManager.purchaseFailedEvent -= AndroidPurchaseFailed;
		
		GoogleIABManager.billingSupportedEvent -= AndroidBillingSupported;
		GoogleIABManager.billingNotSupportedEvent -= AndroidBillingNotSupported;
		
		GoogleIABManager.queryInventorySucceededEvent -= AndroidQueryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent -= AndroidQueryInventoryFailedEvent;
		#else
		#endif
	}
	
	private void FillProductById()
	{
		foreach(gkInAppPurchaseProductID rProduct in products)
		{
			m_oProductById.Add(rProduct.unityProductID, rProduct);
		}
	}
	
	private void LaunchPurchase(gkInAppPurchaseProductID a_rProduct)
	{
		Debug.Log("Launch Purchase : " + a_rProduct.unityProductID);
		if(ms_bDebugFreePurchase)
		{
			OnPurchaseResult(true);
			return;
		}
		
		#if UNITY_EDITOR
		UnityAnalytics.Transaction(a_rProduct.unityProductID, 0.0m, "USD", null, null);
		OnPurchaseResult(true);
		#elif UNITY_IPHONE
		Debug.Log("Launch IOS Purchase : " + a_rProduct.iosProductID);
		StoreKitBinding.purchaseProduct(a_rProduct.iosProductID, 1);
		#elif UNITY_ANDROID
		productTryingToPurchase = a_rProduct.androidProductID;
		Debug.Log("Launch Android Purchase : " + productTryingToPurchase);
		GoogleIAB.purchaseProduct(productTryingToPurchase);
		#else
		OnPurchaseResult(true);
		#endif
	}
	
	private void OnPurchaseResult(bool a_bSuccess, bool a_bCanceledByUser = false)
	{
		Debug.Log("Purchase Result : " + a_bSuccess);
		if(m_rOnPurchaseResult != null)
		{
			m_rOnPurchaseResult(a_bSuccess);
			m_rOnPurchaseResult = null;
		}
		m_bPurchasingInProgress = false;
		
		gkNativeActivityUtility.ShowActivityView(false);
		
		if(a_bSuccess == false && a_bCanceledByUser == false)
		{
			gkNativePopUpManager.Instance.ShowPopUp("", "In app purchase unavailable");
		}
	}
	
	// --- IOS ---
	
	#if UNITY_IPHONE
	private void IOSPurchaseSuccessful(StoreKitTransaction a_rTransaction)
	{
		decimal price = 0.99m;
		StoreKitProduct product;
		if(storeKitProductByID.TryGetValue(a_rTransaction.productIdentifier, out product))
		{
			string priceString = product.price;
			decimal parsedPrice;
			if(decimal.TryParse(priceString, out parsedPrice))
				price = parsedPrice;
		}

		UnityAnalytics.Transaction(a_rTransaction.productIdentifier, price, "USD", a_rTransaction.base64EncodedTransactionReceipt, null);
		OnPurchaseResult(true);
	}	
	
	private void IOSPurchaseFailed(string a_oError)
	{	
		Debug.Log("IOSPurchaseFailed : "  + a_oError);
		OnPurchaseResult(false);
	}
	
	private void IOSPurchaseCancel(string a_oError)
	{
		Debug.Log("IOSPurchaseCancel : "  + a_oError);
		OnPurchaseResult(false, true);
	}
	
	private void IOSProductsReceived(List<StoreKitProduct> a_oStoreKitProducts)
	{
		Debug.Log("Products received");
		foreach(StoreKitProduct rProduct in a_oStoreKitProducts)
		{
			Debug.Log(rProduct);
			storeKitProductByID.Add(rProduct.productIdentifier, rProduct);
		}
		
		m_bInAppAvailable = true;
	}
	
	private void IOSProductsRequestFailed(string a_oError)
	{
		Debug.Log("Products request failed : " + a_oError);
		m_bInAppAvailable = false;
	}
	#endif

	// --- Android ---
	#if UNITY_ANDROID
	private void AndroidPurchaseSuccedeed(GooglePurchase a_rGooglePurchase)
	{
		decimal price = 0.99m;
		GoogleSkuInfo skuInfo;
		if(skuInfosByID.TryGetValue(a_rGooglePurchase.productId, out skuInfo))
		{
			string priceString = skuInfo.price;
			decimal parsedPrice;
			if(decimal.TryParse(priceString, out parsedPrice))
				price = parsedPrice;
		}
		UnityAnalytics.Transaction(a_rGooglePurchase.productId, price, "USD", a_rGooglePurchase.originalJson, a_rGooglePurchase.signature);

		if(consummables.Contains(a_rGooglePurchase.productId))
		{
			GoogleIAB.consumeProduct(a_rGooglePurchase.productId);
		}
		OnPurchaseResult(true);
	}
	
	private void AndroidPurchaseFailed(string a_oError, int response)
	{
		Debug.Log(a_oError);
		if(a_oError.Contains("Item Already Owned"))
		{
			Debug.Log("Item Already Owned " + productTryingToPurchase);
			if(consummables.Contains(productTryingToPurchase))
			{
				Debug.Log("Consume Product Already Owned That Is Consumable " + productTryingToPurchase);
				GoogleIAB.consumeProduct(productTryingToPurchase);
				OnPurchaseResult(false);
			}
			else
			{
				OnPurchaseResult(true);
			}
		}
		else
		{
			if(a_oError.Contains("User canceled"))
			{
				OnPurchaseResult(false, true);
			}
			else
			{
				OnPurchaseResult(false);
			}
		}
	}
	
	private void AndroidBillingSupported()
	{
		Debug.Log("Android Billing Supported");
		string[] oProductIdentifiers = new string[products.Count];
		for(int i = 0; i < products.Count; ++i)
		{
			oProductIdentifiers[i] = products[i].androidProductID;
		}
		GoogleIAB.queryInventory(oProductIdentifiers);
	}
	
	private void AndroidBillingNotSupported(string a_oError)
	{
		Debug.Log("Android Billing Not Supported");
		m_bInAppAvailable = false;
	}
	
	private void AndroidQueryInventorySucceededEvent(List<GooglePurchase> m_rPurchases, List<GoogleSkuInfo> m_rSkuInfos)
	{
		Debug.Log("Inventory succeeded");
		foreach(GoogleSkuInfo skuInfos in m_rSkuInfos)
		{
			Debug.Log("SkuInfos : " + skuInfos);
			skuInfosByID.Add(skuInfos.productId, skuInfos);
		}
		Debug.Log("SkuInfos Listing End");

		m_bInAppAvailable = true;
	}
	
	private void AndroidQueryInventoryFailedEvent(string a_oError)
	{
		Debug.Log("Inventory failed");
		m_bInAppAvailable = false;
	}

	private void ConsumePurchaseFailedEvent(string error)
	{
		Debug.Log("ConsumePurchaseFailedEvent : " + error);
		//OnPurchaseResult(false);
	}

	private void ConsumePurchaseSucceededEvent(GooglePurchase googlePurchase)
	{
		Debug.Log("ConsumePurchaseSucceededEvent : " + googlePurchase);
		//OnPurchaseResult(true);
	}

	#endif
}
//}
#endif
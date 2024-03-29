﻿// /*===============================================================================
// Copyright (C) 2020 PhantomsXR Ltd. All Rights Reserved.
// 
// This file is part of the AR-MOD SDK.
// 
// The AR-MOD SDK cannot be copied, distributed, or made available to
// third-parties for commercial purposes without written permission of PhantomsXR Ltd.
// 
// Contact info@phantomsxr.com for licensing requests.
// ===============================================================================*/


#if UNITY_IOS
using System.Runtime.InteropServices;
#endif
using System;
using UnityEngine;


namespace NativePlugins.Plugin
{
    // ReSharper disable once InconsistentNaming
    public class NativeAPI
    {
        #region Native implement

        public delegate void TryAcquireInformationDelegate(string _data);

        public static TryAcquireInformationDelegate TryAcquireInformationAction;


#if UNITY_IOS && ENABLE_NATIVE && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void throwException(string _errorMessage, int _errorCode);

        [DllImport("__Internal")]
        private static extern void deviceNotSupport();

        [DllImport("__Internal")]
        private static extern void updateLoadingProgress(float _progressValue);

        [DllImport("__Internal")]
        private static extern void addLoadingOverlay();
        
        [DllImport("__Internal")]
        private static extern void removeLoadingOverlay();
        
        [DllImport("__Internal")]
        private static extern void sdkInitialized();
        
        [DllImport("__Internal")]
        private static extern void openBuiltInBrowser(string _url);
        
        [DllImport("__Internal")]
        private static extern void recognitionStart();

        [DllImport("__Internal")]
        private static extern void recognitionComplete();

//        [Obsolete]
//        [DllImport("__Internal")]
//        private static extern string tryAcquireInformation(string _tag);
        
        [DllImport("__Internal")]
        private static extern void packageSizeMoreThanPresetSize(float _currentSize, float _presetSize);
        
        [DllImport("__Internal")]
        private static extern void onARMODExit();
        
        [DllImport("__Internal")]
        private static extern void onARMODLaunch();

        [DllImport("__Internal")]
        private static extern void onMessageReceived(string _data);
        
        [DllImport("__Internal")]
        private static extern void tryAcquireInformation(string _tag,TryAcquireInformationDelegate _callback);

        
        [AOT.MonoPInvokeCallback(typeof(TryAcquireInformationDelegate))]
        private static void TryAcquireInformationCallback(string _info)
        {
           TryAcquireInformationAction?.Invoke(_info);
        }
        
#elif UNITY_ANDROID && ENABLE_NATIVE && !UNITY_EDITOR
        private static AndroidJavaObject androidJavaObject;
        private const string CONST_PACKAGE_NAME = "com.phantomsxr.armodplugin.ARMODCallbackAPI";
        private const string CONST_NOT_SUPPORT_ARMODE = "deviceNotSupport";
        private const string CONST_REMOVE_LOADING_OVERLAY = "removeLoadingOverlay";
        private const string CONST_ADD_LOADING_OVERLAY = "addLoadingOverlay";
        private const string CONST_UPDATE_LOADING_PROGRESS = "updateLoadingProgress";
        private const string CONST_ALERT = "throwException";
        private const string CONST_NEED_INSTALL_ARCORE_SERVICE = "needInstallARCoreService";
        private const string CONST_AR_ALGORITHM_INITIALIZED = "sdkInitialized";
        private const string CONST_OPEN_BUILTIN_BROWSER = "openBuiltInBrowser";
        private const string CONST_RECOGNITION_START = "recognitionStart";
        private const string CONST_RECOGNITION_COMPLETE = "recognitionComplete";
        private const string CONST_TRY_ACQUIRE_INFORMATION = "tryAcquireInformation";
        private const string CONST_PACKAGE_SIZE_MORE_THAN_PRESET_SIZE = "packageSizeMoreThanPresetSize";
        private const string CONST_ON_ARMOD_EXIT = "onARMODExit";
        private const string CONST_ON_ARMOD_LAUNCH = "onARMODLaunch";
        private const string CONST_ON_MESSAGE_RECEVIED="onMessageReceived";

        class AndroidCallback:AndroidJavaProxy
        {
            public AndroidCallback() : base("com.phantomsxr.armodplugin.AndroidCallback") { }



            void TryAcquireInformationCallback(string _info)
            {
                TryAcquireInformationAction?.Invoke(_info);
            }
        }


        public static AndroidJavaObject GetAndroidJavaObject(string _packageNameWithActivity)
        {
            if (androidJavaObject != null) return androidJavaObject;
            AndroidJavaClass tmp_JavaClass = new AndroidJavaClass(_packageNameWithActivity);
            androidJavaObject = tmp_JavaClass.GetStatic<AndroidJavaObject>("instance");

            return androidJavaObject;
        }
#endif

        #endregion


        #region Native Unity Events

#if !ENABLE_NATIVE || UNITY_EDITOR
        public static event Action DeviceNotSupportEventHandle;

        public static event Action AddLoadingOverlayEventHandle;

        public static event Action<float> UpdateLoadingProgressEventHandle;

        public static event Action RemoveLoadingOverlayEventHandle;

        public static event Action<string, int> ThrowExceptionEventHandle;


        public static event Action SdkInitializedEventHandle;

        public static event Action<string> OpenBuiltInBrowserEventHandle;

        public static event Action RecognitionStartEventHandle;

        public static event Action RecognitionCompleteEventHandle;

        public static event Action<string, TryAcquireInformationDelegate> TryAcquireInformationEventHandle;

#pragma warning disable CS0067
        public static event Action NeedInstallARCoreServicesEventHandle;
#pragma warning restore CS0067

        public static event Action<float, float> PackageSizeMoreThanPresetSizeEventHandle;

        public static event Action OnARMODExitEventHandle;

        public static event Action OnARMODLaunchEventHandle;
        
        public static event Action<string> OnMessageReceviedHandle;
#endif

        #endregion

        public static void DeviceNotSupport()
        {
#if UNITY_IOS && ENABLE_NATIVE && !UNITY_EDITOR
            deviceNotSupport();
#elif UNITY_ANDROID && ENABLE_NATIVE && !UNITY_EDITOR
            GetAndroidJavaObject(CONST_PACKAGE_NAME).Call(CONST_NOT_SUPPORT_ARMODE);
#else
            DeviceNotSupportEventHandle?.Invoke();
#endif
        }

        public static void AddLoadingOverlay()
        {
#if UNITY_IOS && ENABLE_NATIVE && !UNITY_EDITOR
            addLoadingOverlay();
#elif UNITY_ANDROID && ENABLE_NATIVE && !UNITY_EDITOR
            GetAndroidJavaObject(CONST_PACKAGE_NAME).Call(CONST_ADD_LOADING_OVERLAY);
#else
            AddLoadingOverlayEventHandle?.Invoke();
#endif
        }

        public static void UpdateLoadingProgress(float _progressValue)
        {
#if UNITY_IOS && ENABLE_NATIVE && !UNITY_EDITOR
            updateLoadingProgress(_progressValue);
#elif UNITY_ANDROID && ENABLE_NATIVE && !UNITY_EDITOR
            GetAndroidJavaObject(CONST_PACKAGE_NAME).Call(CONST_UPDATE_LOADING_PROGRESS, _progressValue);
#else
            UpdateLoadingProgressEventHandle?.Invoke(_progressValue);
#endif
        }

        public static void RemoveLoadingOverlay()
        {
#if UNITY_IOS && ENABLE_NATIVE && !UNITY_EDITOR
            removeLoadingOverlay();
#elif UNITY_ANDROID && ENABLE_NATIVE && !UNITY_EDITOR
            GetAndroidJavaObject(CONST_PACKAGE_NAME).Call(CONST_REMOVE_LOADING_OVERLAY);
#else
            RemoveLoadingOverlayEventHandle?.Invoke();
#endif
        }

        public static void ThrowException(string _errorMessage, int _errorCode)
        {
#if UNITY_IOS && ENABLE_NATIVE && !UNITY_EDITOR
            throwException(_errorMessage,_errorCode);
#elif UNITY_ANDROID && ENABLE_NATIVE && !UNITY_EDITOR
            GetAndroidJavaObject(CONST_PACKAGE_NAME).Call(CONST_ALERT, _errorMessage, _errorCode);
#else
            ThrowExceptionEventHandle?.Invoke(_errorMessage, _errorCode);
#endif
        }

        public static void SdkInitialized()
        {
#if UNITY_IOS && ENABLE_NATIVE && !UNITY_EDITOR
            sdkInitialized();
#elif UNITY_ANDROID && ENABLE_NATIVE && !UNITY_EDITOR
            GetAndroidJavaObject(CONST_PACKAGE_NAME).Call(CONST_AR_ALGORITHM_INITIALIZED);
#else
            SdkInitializedEventHandle?.Invoke();
#endif
        }

        public static void OpenBuiltInBrowser(string _url)
        {
#if UNITY_IOS && ENABLE_NATIVE && !UNITY_EDITOR
            openBuiltInBrowser(_url);
#elif UNITY_ANDROID && ENABLE_NATIVE && !UNITY_EDITOR
             GetAndroidJavaObject(CONST_PACKAGE_NAME).Call(CONST_OPEN_BUILTIN_BROWSER, _url);
#else
            OpenBuiltInBrowserEventHandle?.Invoke(_url);
#endif
        }

        public static void RecognitionStart()
        {
#if UNITY_IOS && ENABLE_NATIVE && !UNITY_EDITOR
            recognitionStart();
#elif UNITY_ANDROID && ENABLE_NATIVE && !UNITY_EDITOR
            GetAndroidJavaObject(CONST_PACKAGE_NAME).Call(CONST_RECOGNITION_START);
#else
            RecognitionStartEventHandle?.Invoke();
#endif
        }

        public static void RecognitionComplete()
        {
#if UNITY_IOS && ENABLE_NATIVE && !UNITY_EDITOR
            recognitionComplete();
#elif UNITY_ANDROID && ENABLE_NATIVE&& !UNITY_EDITOR
            GetAndroidJavaObject(CONST_PACKAGE_NAME).Call(CONST_RECOGNITION_COMPLETE);
#else
            RecognitionCompleteEventHandle?.Invoke();
#endif
        }

        public static void TryAcquireInformation(string _tag, TryAcquireInformationDelegate _action)
        {
            TryAcquireInformationAction = _action;

#if UNITY_IOS && ENABLE_NATIVE && !UNITY_EDITOR
            tryAcquireInformation(_tag,TryAcquireInformationCallback);
#elif UNITY_ANDROID && ENABLE_NATIVE&& !UNITY_EDITOR
            GetAndroidJavaObject(CONST_PACKAGE_NAME).Call(CONST_TRY_ACQUIRE_INFORMATION,_tag,new AndroidCallback());
#else
            TryAcquireInformationEventHandle?.Invoke(_tag, _action);
#endif
        }

        public static void NeedInstallARCoreServices()
        {
#if UNITY_IOS && ENABLE_NATIVE
            //iOS does not use ARCore.
#elif UNITY_ANDROID && ENABLE_NATIVE&& !UNITY_EDITOR
            GetAndroidJavaObject(CONST_PACKAGE_NAME).Call(CONST_NEED_INSTALL_ARCORE_SERVICE);
#else
            NeedInstallARCoreServicesEventHandle?.Invoke();
#endif
        }

        public static void PackageSizeMoreThanPresetSize(float _currentSize, float _presetSize)
        {
#if UNITY_IOS && ENABLE_NATIVE && !UNITY_EDITOR
            packageSizeMoreThanPresetSize(_currentSize,_presetSize);
#elif UNITY_ANDROID && ENABLE_NATIVE&& !UNITY_EDITOR
            GetAndroidJavaObject(CONST_PACKAGE_NAME).Call(CONST_PACKAGE_SIZE_MORE_THAN_PRESET_SIZE,_currentSize,_presetSize);
#else
            PackageSizeMoreThanPresetSizeEventHandle?.Invoke(_currentSize, _presetSize);
#endif
        }

        public static void OnXRMODExit()
        {
#if UNITY_IOS && ENABLE_NATIVE && !UNITY_EDITOR
            onARMODExit();
#elif UNITY_ANDROID && ENABLE_NATIVE&& !UNITY_EDITOR
            GetAndroidJavaObject(CONST_PACKAGE_NAME).Call(CONST_ON_ARMOD_EXIT);
#else
            OnARMODExitEventHandle?.Invoke();
#endif
        }

        public static void OnXrModLaunch()
        {
#if UNITY_IOS && ENABLE_NATIVE && !UNITY_EDITOR
            onARMODLaunch();
#elif UNITY_ANDROID && ENABLE_NATIVE&& !UNITY_EDITOR
            GetAndroidJavaObject(CONST_PACKAGE_NAME).Call(CONST_ON_ARMOD_LAUNCH);
#else
            OnARMODLaunchEventHandle?.Invoke();
#endif
        }

        /// <summary>
        /// Send a data(string) to XR-MOD Launcher
        /// </summary>
        /// <param name="_data">Will send message data</param>
        public static void SendMessageToSDK(string _data)
        {
#if UNITY_IOS && ENABLE_NATIVE && !UNITY_EDITOR
            onMessageReceived(_data);
#elif UNITY_ANDROID && ENABLE_NATIVE&& !UNITY_EDITOR
            GetAndroidJavaObject(CONST_PACKAGE_NAME).Call(CONST_ON_MESSAGE_RECEVIED,_data);
#else
            OnMessageReceviedHandle?.Invoke(_data);
#endif
        }
    }
}
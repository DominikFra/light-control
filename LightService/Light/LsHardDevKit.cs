using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;


namespace LightService
{
    using DHDK_DEVICE_HANDLE = System.UIntPtr;
    using DHDK_DMX_UNIVERSE_HANDLE = System.UIntPtr;
    using DHDK_UINT = System.UIntPtr;
    

    static class DHDK_CONST
    {
        public const int StringBufferSize = 256;
    }

    [Flags]
    public enum Protocols
    {
        DASUSB = 1,
        DASNET = 2,
        ARTNET = 4,
    }

    public static class DmxInterface
    {
        /**
        * First Function to call on start up
        * @warning this function is not treadsafe, and should not be called during any other function can be acceced
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void DHDK_init(UIntPtr protocols);

        public const uint UINT_ERROR = 0xFFFFFFFF;
        public static readonly DHDK_UINT DHDK_UINT_ERROR = UIntPtr.Subtract(UIntPtr.Zero,1);
        public static readonly DHDK_UINT DHDK_INVLIDE_HANDLE = UIntPtr.Subtract(UIntPtr.Zero, 1);
        
        public enum ErrorCode
        {
            XHL_Error_NotSet = -1,

            XHL_Error_NoError = 0,
            XHL_Error_UnkownError,     ///< 1 for unkown

            XHL_Error_System_Network_LibraryNotInitilised,  ///< 2 For Windows only, probleme during WSA inisialisation
            XHL_Error_XHL_InternalNetworkError,             ///< 3 raised internally by XHL, when ethernet comunication is working whell, but unexpected result occured
            XHL_Error_XHL_InvalidSocket,                    ///< 4 
            XHL_Error_System_Network_ConnexionRefused,      ///< 5
            XHL_Error_System_Network_TimeOut,               ///< 6
            XHL_Error_System_BrokenPipe,                    ///< 7
            XHL_Error_System_InvalidSlot,                   ///< 8
            XHL_Error_System_BadFile,                       ///< 9
            XHL_Error_System_PermissionDenied,              ///< 10
            XHL_Error_System_OutOfMemory,                   ///< 11
            XHL_Error_XHL_DeviceNotOpen,                    ///< 12 try to perform action on a closed device
            XHL_Error_System_Network_ConnectionDisconnectByPeer, ///< 13
            XHL_Error_XHL_DeviceDisconnected,               ///< 14
            XHL_Error_XHL_InvalideUsbTransfert,             ///< 15
            XHL_Error_XHL_DeviceAlreadyUsed,                ///< 16 device is own by another application
            XHL_Error_XHL_TimeOut,                          ///< 17 time out 
            XHL_Error_XHL_InternalUsbError,                 ///< 18  raised internally to xhl, when USB resquest are properly performed, but unexpected result occured
            XHL_Error_XHL_FunctionalityNotSupportedByDevice,///< 19
            XHL_Error_XHL_ShowSizeIsTooBig,                 ///< 20
            XHL_Error_XHL_ShowContainsTooMuchScene,         ///< 21
            XHL_Error_XHL_ShowContainsTooMuchTimeTrigger,   ///< 22
            XHL_Error_OutOfBounds,                          ///< 23
            XHL_Error_RTFM,                                 ///< 24 Read That funking Manual, generally all stupid error programmer can do without checking he can do it
            XHL_Error_XHL_DongleLimitation,                 ///< 25 try to perfome an operatioin not allowed due to dongle limitation
            XHL_Error_XHL_BadShowVersion,                   ///< 26 try to do operation on a version of show not suported
            XHL_Error_XHL_UnableToLoadShow,                 ///< 27 cannot load show, show may be cooupted
            XHL_Error_InvalidArgument,                      ///< 28 argument pased to XHL are not correct
            XHL_Error_TryAgain,                             ///< 29 user and or juste uper layer than XHL  should try again (or OS generated error not catch by XHL)
            XHL_Error_InterruptedSystemCall,                ///< 30
            XHL_Error_OperationNotPermitted,                ///< 31
            XHL_Error_System_Network_AddressAlreadyInUse,   ///< 32 Another program use already this network address (ip adress + port) Many XHL based program can be open together, socket will be shared, but XHL with some other program that don't share this socket can cause error.
            XHL_Error_FileNotFound,                         ///< 33
            XHL_Error_XHL_CommunicationError,               ///< 34 device don't reply as expected
            XHL_Error_XHL_DeviceAlreadyOpen,                ///< 35 the device is already open by curent XHL
            XHL_Error_XHL_FirmwareTooOld,                   ///< 36
            XHL_Error_XHL_FirmwareTooRecent,                ///< 37
            XHL_Error_System_Network_is_unreachable,        ///< 38 (system error) no network adapter avaible (Windows XP error)
            XHL_Error_System_Network_Insufficient_buffer_space, ///< 39
            XHL_Error_System_InvalidHandle,                 ///< 40 (system error) XHL try to use an invalid OS handle (genraly should be catch by XHL)
            XHL_Error_System_Network_No_route_to_host,      ///< 41
            XHL_Error_System_BadCommand,                    ///< 42
            XHL_Error_XHL_ShowCannotBeEmpty,                ///< 43
            XHL_Error_XHL_InternalUsbErrorPipe,             ///< 44 usb request ignored by device, malformed request, device cannot handle this request...
            XHL_Error_System_GenFailure,                    ///< 45 (system error) occure sometime on windows when unplug an USB device
            XHL_Error_System_IO,                            ///< 46 (system error) occure sometime on windows when unplug an USB device
            XHL_Error_System_InvalidAccessToMemoryLocation, ///< 47 (system error)
            XHL_Error_XHL_ShowFrequencyIsNotSupported,      ///< 48
            XHL_Error_XHL_ShowContaintsTwoMuchZone,         ///< 49
            XHL_Error_XHL_ShowContaintsTwoMuchPages,        ///< 50
            XHL_Error_XHL_CsaShowCannoContaintsEmptyScene,  ///< 51
            XHL_Error_System_Network_ConnectionAbordedLocaly,///< 52
            XHL_Error_XHL_NapLimitation,                    ///< 53
            XHL_Error_XHL_InternalError,                    ///< 54 unexpedted stuf occure
            XHL_Error_System_Network_SocketAlreadyClosed,   ///< 55 (system errror) Sould be catch by XHL
            XHL_Error_XHL_MemoryIsEmpty,                    ///< 56 No SDcard
            XHL_Error_System_Operation_now_in_progress,     ///< 57 (system errror) Sould be catch by XHL
            XHL_Error_XHL_ShowTooMuchChannels,              ///< 58
            XHL_Error_System_Network_HostIsDown,            ///< 59
            XHL_Error_System_Class_Already_Exists,          ///< 60 (system errror) Sould be catch by XHL
            XHL_Error_XHL_UsbtransfertCanceled,             ///< 61 (system errror) Sould be catch by XHL
            XHL_Error_AlmostOneBusFailledToEnumerate,       ///< 62 genery error for multiple bus enumeration
            XHL_Error_XHL_OperationCannontBePerformedFromThisThread,///< 63 Programe, (or XHL) try to perform request from invalid thread
            XHL_Error_XHL_TestFailled,                      ///< 64, generic error when a test during production failled
            XHL_Error_XHL_WrongSerialNumber,                ///< 65, device serial number is not in the expected range
            XHL_Error_XHL_TokenExpired,                     ///< 66, a production token is expired
            DHDK_Error_InvalidArguement,                    ///< 67 for DevKit layer: invalide argument
            DHDK_Error_InvalidHandle,                       ///< 68 for DevKit layer: invalide handle
            DHDK_Error_NotInitialized,                      ///< 69 for DevKit layer: try to use devkit withou initialize it
            DHDK_Error_HandleNoMoreValide,                  ///< 70 for DevKit layer: object associated to this handle has been destroyed
            XHL_Error_XHL_MissingFont,                      ///< 71 XHL cannot load font when wirting a show (generally font.ttc file in working directory)

            //sould be alwais the last
            XHL_Error_Count,//used for internal compilation


        };
       

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetDllDirectory(String lpPathName);
            

        public static void init(Protocols protocols)
        {
            SetDllDirectory(AppDomain.CurrentDomain.BaseDirectory + "/"+System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")+"/");
            DHDK_init(new UIntPtr(Convert.ToUInt32(protocols)));
        }

        /**
        * To Call when  when your aplication is living
        * @warning this function is not treadsafe,  and should not be called during any other function can be acceced
        */
        [DllImport("LsHardDevKit.dll",EntryPoint = "DHDK_deinit", CallingConvention = CallingConvention.StdCall)]
        public static extern void deinit();

        /**
        *list de device avaible on your system
        */
        [DllImport("LsHardDevKit.dll", EntryPoint = "DHDK_enumerate", CallingConvention = CallingConvention.StdCall)]    
        public static extern bool enumerate();


        /**
        /*return number of device enumerate on your system
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern UIntPtr DHDK_getDeviceCount();   
        
        public static uint getDeviceCount()
        {
            return DHDK_getDeviceCount().ToUInt32();
        }

        /**
        *give an Handle to a device
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern DHDK_DEVICE_HANDLE DHDK_getDevice(UIntPtr iDevice);
        
        static private Dictionary<DHDK_DEVICE_HANDLE, Device> _deviceDic = new Dictionary<DHDK_DEVICE_HANDLE, Device>();
    
        public static Device getDevice(uint iDevice)
        {
            DHDK_DEVICE_HANDLE hDevice = DHDK_getDevice(new UIntPtr(iDevice));
            Device device;
            if(!_deviceDic.TryGetValue(hDevice, out device))
            {
                device = new Device(hDevice);
                _deviceDic.Add(hDevice, device);
            }

            return device;
        }


        /**
        * @brief getLastError
        * @return the last error raised by calling thread
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern UIntPtr DHDK_getLastError();
        static public ErrorCode getLastError()
        {
            return (ErrorCode)(DHDK_getLastError());
        }
        

    }



    public class Device
    {

        private DHDK_DEVICE_HANDLE _hDevice;

        public Device(DHDK_DEVICE_HANDLE hDevice)
        {
            _hDevice = hDevice;
        }

        /**
        * Enables to open the communication with the device
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private extern static bool DHDK_openDevice(DHDK_DEVICE_HANDLE hDevice );
        public bool openDevice()
        {
            return DHDK_openDevice(_hDevice);
        }

        /**
        * Enables to stop the communication with the device
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private extern static bool DHDK_closeDevice(DHDK_DEVICE_HANDLE hDevice );
        public void close()
        {
            DHDK_closeDevice(_hDevice);
        }

        /**
        * @brief DHDK_getDeviceTypeName return the name of the type of the device
        * @param[in] hDevice handle of the device to query the name
        * @param[out] a pointer wher wil be filled device name in utf8 codding
        * @param[in] size of buffer in char
        * @return
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool DHDK_getDeviceTypeName(DHDK_DEVICE_HANDLE hDevice, StringBuilder lpString, UIntPtr size);

        public bool getTypeName(out String aName)
        {
            StringBuilder sb = new StringBuilder(DHDK_CONST.StringBufferSize);
            if (!DHDK_getDeviceTypeName(_hDevice, sb, new UIntPtr(unchecked((uint)sb.Capacity))))
            {
                aName = "";
                return false;
            }
            aName = sb.ToString();
            return true;
        }


        /**
        * @brief DHDK_getDeviceName return the name of the device
        * @param[in] hDevice handle of the device to query the name
        * @param[out] a pointer wher wil be filled device name in utf8
        * @param[in] size in char
        * @return
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool DHDK_getDeviceName(DHDK_DEVICE_HANDLE hDevice, StringBuilder lpString, UIntPtr size);

        public bool getName(out String aName)
        {
            StringBuilder sb = new StringBuilder(DHDK_CONST.StringBufferSize);
            if (!DHDK_getDeviceName(_hDevice, sb, new UIntPtr(unchecked((uint)sb.Capacity))))
            {
                aName = "";
                return false;
            }
            aName = sb.ToString();
            return true;
        }


        /**
        /*return the Device serial
        */
        [DllImport("LsHardDevKit.dll", EntryPoint = "DHDK_getDeviceSerial", CallingConvention = CallingConvention.StdCall)]
        private static extern UIntPtr DHDK_getDeviceSerial(DHDK_DEVICE_HANDLE hDevice);

        public uint getSerial()
        {
            return DHDK_getDeviceSerial(_hDevice).ToUInt32();
        }


        [DllImport("LsHardDevKit.dll", EntryPoint = "DHDK_getDeviceProtocol", CallingConvention = CallingConvention.StdCall)]
        private static extern UIntPtr DHDK_getDeviceProtocol(DHDK_DEVICE_HANDLE hDevice);

        public Protocols getProtocol()
        {
            return (Protocols)DHDK_getDeviceProtocol(_hDevice);

        }

        /**
        /* return bitfield for active port: \n
        * bit 0 is set if port 1 active, \n
        * bit 1,...
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern UIntPtr DHDK_getPort(DHDK_DEVICE_HANDLE hDevice);

        public uint getPort()
        {
            return DHDK_getPort(_hDevice).ToUInt32();
        }



        /**
        /*return number of button on the device
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern UIntPtr DHDK_getButtonCount(DHDK_DEVICE_HANDLE hDevice);
        public uint getButtonCount()
        {
            return DHDK_getButtonCount(_hDevice).ToUInt32();
        }


        /**
        *return state of button on the device
        * @return 0 means button leave\n
        * 1 means button pressed
        * UIntPtr_ERROR mean an erro occured when reading port
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern UIntPtr DHDK_getButtonState(DHDK_DEVICE_HANDLE hDevice, UIntPtr iButton);
        public uint getButtonState(uint iButton)
        {
            return DHDK_getButtonState(_hDevice, new UIntPtr(iButton)).ToUInt32();
        }

        /**
        * return bool is active iButton button is active
        * @param[in] hDevice handle of the device to get Button information
        * @param[in] iButton the index of the Button
        * @param[out] a pointer wher wil be filled button name in utf8 codding, size
        * @param[in] size of buffer in char
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool DHDK_getButtonName(DHDK_DEVICE_HANDLE hDevice, UIntPtr iButton, StringBuilder lpString, UIntPtr size);
        public bool getButtonName( uint iButton, out String aName)
        {
            StringBuilder sb = new StringBuilder(DHDK_CONST.StringBufferSize);
            if (!DHDK_getButtonName(_hDevice, new UIntPtr(iButton), sb, new UIntPtr(unchecked((uint)sb.Capacity))))
            {
                aName = "";
                return false;
            }
            aName = sb.ToString();
            return true;
        }

        /**
        * Return the number of Dmx Universe suported by the Device
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern UIntPtr DHDK_getDmxUniverseCount(DHDK_DEVICE_HANDLE hDdevice);
        public uint getDmxUniverseCount()
        {
            return DHDK_getDmxUniverseCount(_hDevice).ToUInt32();
        }




        /**
        *return handle for universe
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern DHDK_DMX_UNIVERSE_HANDLE DHDK_getDmxUniverse(DHDK_DEVICE_HANDLE hDevice, UIntPtr univeNumber);

        private Dictionary<DHDK_DMX_UNIVERSE_HANDLE, DmxUniverse> _universeDic = new Dictionary<DHDK_DMX_UNIVERSE_HANDLE, DmxUniverse>();
        public  DmxUniverse getDmxUnivers(uint univeNumber)
        {
            DHDK_DMX_UNIVERSE_HANDLE hUniverse = DHDK_getDmxUniverse(_hDevice, new UIntPtr(univeNumber));
            DmxUniverse result = null;
            if (hUniverse != DmxInterface.DHDK_INVLIDE_HANDLE)
            {
                if (!_universeDic.TryGetValue(hUniverse, out result))
                {
                    result = new DmxUniverse(hUniverse);
                    _universeDic.Add(hUniverse, result);
                }
            }
            else
            {
                result = null;
            }

            return result;
        }
    }


    public class DmxUniverse
    {
        DHDK_DMX_UNIVERSE_HANDLE _hDmxUniverse;

        [Flags] public enum WorkingMode
        {
            DmxOut = 1,
            DmxIn = 2,
        }
        


        public DmxUniverse(DHDK_DMX_UNIVERSE_HANDLE aDmxUniverseHandle)
        {
            _hDmxUniverse = aDmxUniverseHandle;
        }

        /**
        * give suport of Dmx universe has bitfield
        * @code
        if(universe.getDmxUniverseFeature(hDmxUniverse) & DHDK_DMXOUT)
        {
        //universe suport DmxOut
        * universe.sendDmx(....)
        }
        @endcode
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern UIntPtr DHDK_getDmxUniverseFeatures(DHDK_DMX_UNIVERSE_HANDLE hDmxUniverse);
        public WorkingMode getSuportedFeatures()
        {
            return (WorkingMode) DHDK_getDmxUniverseFeatures(_hDmxUniverse);
        }

        /**
        * configure a Dmx universe has output or input
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool DHDK_configureDmxUniverse(DHDK_DMX_UNIVERSE_HANDLE hDmxUniverse, WorkingMode mode);

        public bool configureAs(WorkingMode aWorkingMode)
        {
            return DHDK_configureDmxUniverse(_hDmxUniverse, aWorkingMode);
        }

        /**
        * sendDmx
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool DHDK_sendDmx(DHDK_DMX_UNIVERSE_HANDLE hDmxUniverse , byte[] dmxBuffer, UIntPtr size);
        
        public bool sendDmx(byte [] dmxBuffer)
        {
            return DHDK_sendDmx( _hDmxUniverse, dmxBuffer, new UIntPtr((uint)dmxBuffer.Length));
        }


        /**
        * pool dmxReceive Buffer
        * @param dmxBuffer pointer to a 512 bytes buffer
        * @return number of bytes copied into dmxBuffer
        */
        [DllImport("LsHardDevKit.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool DHDK_receiveDmx(DHDK_DMX_UNIVERSE_HANDLE hDmxUniverse, IntPtr dmxBuffer, UIntPtr size);
        
        public bool receveivDmx(ref byte [] dmxBuffer)
        {
            //TODO: propose better way to pass buffer to a dll without copying it
            IntPtr  unmanagedbuffer = Marshal.AllocHGlobal(dmxBuffer.Length);
            bool result = DHDK_receiveDmx(_hDmxUniverse, unmanagedbuffer, new UIntPtr((uint)dmxBuffer.Length));
            Marshal.Copy(unmanagedbuffer, dmxBuffer, 0, dmxBuffer.Length);
            
            return result;
        }



    }


};
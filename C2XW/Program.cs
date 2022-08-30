using C2XW.Models;
using Emgu.CV.XFeatures2D;
using Newtonsoft.Json;
using RestSharp;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;


namespace C2XW
{
    internal class Program
    {

        [DllImport("user32.dll")]
        internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        internal static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        internal static extern bool SetClipboardData(uint uFormat, IntPtr data);

        public static  string lineToken = "";
        public static string deviceID = null;
        public static List<Account> Data_Account;


        public static Bitmap Recover_BMP;
        public static Bitmap Seed_BMP;
        public static Bitmap Paste_BMP;
        public static Bitmap Next_BMP;
        public static Bitmap WalletName_BMP;
        public static Bitmap Explore_BMP;
        public static Bitmap Convert_BMP;
        public static Bitmap IconGame_BMP;
        public static Bitmap TextConvert_BMP;
        public static Bitmap Agree_BMP;
        public static Bitmap One_BMP;
        public static Bitmap Ven_BMP;
        public static Bitmap Login_BMP;
        public static Bitmap Email_BMP;
        public static Bitmap IDLE_BMP;
        public static Bitmap ICONC2X_BMP;
        public static Bitmap BAG_BMP;
        public static Bitmap DeleteWallet_BMP;
        public static Bitmap Error_BMP;

        private static string INPUT_TEXT_DEVICES = "adb -s {0} shell input text \"{1}\"";
        private static string ADB_FOLDER_PATH = "";

        private static string OPEN_APP_DEVICES = "adb -s {0} shell monkey -p {1} -c android.intent.category.LAUNCHER 1";
        private static string Kill_APP_DEVICES = "adb -s {0} shell am force-stop {1}";



        public static bool CheckClickRecover;
        public static bool CheckClickUseSeed;
        public static bool CheckInputMneni;
        public static bool CheckInputNameWallet;
        public static bool CheckClickExploer;
        public static bool CheckClickConvert;
        public static bool CheckClickAgree;
        public static bool CheckFoundVen;
        public static bool CheckClickIconGame;
        public static bool CheckClickOne;
        public static bool CheckSentOTP;
        public static bool CheckClickLogin;
        public static bool CheckClickEmail;
        public static bool CheckClickIDLE;
        public static bool CheckClickBagWallet;
        public static bool CheckError;

        async static Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Start C2XW link account wallet.");

            Load();
            Run();
        }


        public static void Load()
        {
            LoadDevices();
            LoadBMP();
            LoadJson();


        }

        public static void LoadBMP()
        {
            //Wallet_BMP = (Bitmap)Bitmap.FromFile("Data//wallet.png");
            try
            {
                Recover_BMP = (Bitmap)Bitmap.FromFile("bmp//recover.png");
                Seed_BMP = (Bitmap)Bitmap.FromFile("bmp//seed.png");
                Paste_BMP = (Bitmap)Bitmap.FromFile("bmp//paste.png");
                Next_BMP = (Bitmap)Bitmap.FromFile("bmp//next.png");
                WalletName_BMP = (Bitmap)Bitmap.FromFile("bmp//Wallet_Name.png");
                Explore_BMP = (Bitmap)Bitmap.FromFile("bmp//explore.png");
                Convert_BMP = (Bitmap)Bitmap.FromFile("bmp//convert.png");
                IconGame_BMP = (Bitmap)Bitmap.FromFile("bmp//icon_game.png");
                TextConvert_BMP = (Bitmap)Bitmap.FromFile("bmp//text_convert.png");
                Agree_BMP = (Bitmap)Bitmap.FromFile("bmp//agree.png");
                One_BMP = (Bitmap)Bitmap.FromFile("bmp//one.png");
                Ven_BMP = (Bitmap)Bitmap.FromFile("bmp//84.png");
                Login_BMP = (Bitmap)Bitmap.FromFile("bmp//login.png");
                Email_BMP = (Bitmap)Bitmap.FromFile("bmp//email.png");
                IDLE_BMP = (Bitmap)Bitmap.FromFile("bmp//back.png");
                ICONC2X_BMP = (Bitmap)Bitmap.FromFile("bmp//icon_c2x.png");
                BAG_BMP = (Bitmap)Bitmap.FromFile("bmp//bag.png");
                DeleteWallet_BMP = (Bitmap)Bitmap.FromFile("bmp//delete.png");
                Error_BMP = (Bitmap)Bitmap.FromFile("bmp//error_signature.png");
            }
            catch (Exception e)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + e);
            }


        }

        public static void LoadJson()
        {
            string json = System.IO.File.ReadAllText("account.json");
            Data_Account = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Account>>(json);

        }

        public static void LoadDevices()
        {

            var listDevice = KAutoHelper.ADBHelper.GetDevices();

            if (listDevice != null && listDevice.Count > 0)
            {
                deviceID = listDevice.First();
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Connect devices : " + deviceID);

            }
            else
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Not have devices");
            }
        }

        public static string GetSubstringByString(string a, string b, string c)
        {
            return c.Substring((c.IndexOf(a) + a.Length), (c.IndexOf(b) - c.IndexOf(a) - a.Length));
        }

        public static void lineNotifyMessage(string msg)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient("https://notify-api.line.me/api/notify");
            client.Timeout = 10000;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + lineToken);
            request.AddHeader("Content-type", "application/x-www-form-urlencoded");
            request.AddParameter("message", msg);
            var response = client.Execute(request);
            client.Timeout = 10000;
            if (response.StatusCode == HttpStatusCode.OK)
            {
              
                

            }

        }

        public static async Task Run()
        {
            

            if (deviceID != null)
            {

                var writer = new StreamWriter("filename.txt");
                foreach (Account key in Data_Account)
                {
                repeat2:
                    //OpenApp(deviceID, "c2xstation.android");
                    var status = true;
                    CheckInputMneni = false;
                    CheckInputNameWallet = false;
                    CheckClickExploer = false;
                    CheckClickConvert = false;
                    
                    CheckClickUseSeed = false;
                    CheckClickRecover = false;

                    CheckClickAgree = false;
                    CheckFoundVen = false;
                    CheckClickIconGame = true;
                    CheckClickOne=false;
                    CheckSentOTP = false;
                    CheckClickEmail = false;
                    CheckClickLogin = false;
                    CheckClickIDLE = false;
                    CheckClickBagWallet = false;
                    CheckError = false;


                    Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Start " + key.email);
                    while (status)
                    {
                        //Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] I " + i);
                        if (!CheckClickRecover)
                        {
                            SearchRecover(deviceID, Recover_BMP);
                        }

                        if (!CheckClickUseSeed)
                        {
                            SearchUseSeed(deviceID, Seed_BMP);
                        }


                        if (CheckInputMneni == false && CheckClickRecover == true)
                        {
                            SearchPaste(deviceID, Paste_BMP, key.mnemonic);
                            var count = 0;
                            var loopNext = true;
                            while (loopNext)
                            {
                                var checkPointNext = SearchNext(deviceID, Next_BMP);
                                if (checkPointNext == true)
                                {
                                    loopNext = false;
                                }
                                else if (count > 15)
                                {
                                    KillApp(deviceID, "c2xstation.android");
                                    Thread.Sleep(1000);
                                    goto repeat2;
                                }
                                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Count " + count);
                                Thread.Sleep(1000);
                                count++;
                            }

                        }
                        if (CheckInputNameWallet == false && CheckClickRecover == true && CheckInputMneni == true)
                        {
                            SearchWalletName(deviceID, WalletName_BMP, key.email);
                            bool Next1 = true;
                            while (Next1)
                            {
                                var checkPointNext = SearchNext(deviceID, Next_BMP);
                                if (checkPointNext == true)
                                {
                                    Next1 = false;
                                }
                            }
                        }
                        if (!CheckClickExploer && CheckClickRecover == true && CheckInputMneni == true && CheckInputNameWallet == true)
                        {
                            SearchExplore(deviceID, Explore_BMP);
                        }

                        if (!CheckClickConvert && CheckClickRecover == true && CheckInputMneni == true && CheckInputNameWallet == true && CheckClickExploer == true)
                        {
                            SearchConvert(deviceID, Convert_BMP);
                        }



                        if (CheckClickConvert && CheckClickRecover == true && CheckInputMneni == true && CheckInputNameWallet == true && CheckClickExploer == true && CheckClickAgree == false)
                        {
                            SearchAccept(deviceID, Agree_BMP);
                        }


                        var checkTextConvert = SearchTextConvert(deviceID, TextConvert_BMP);
                        if (checkTextConvert == true)
                        {
                            SearchIconGame(deviceID, IconGame_BMP);
                        }


                        if (CheckClickIconGame && CheckSentOTP == false)
                        {
                            SearchOne(deviceID, One_BMP);
                            if (CheckClickOne)
                            {
                                SearchVen(deviceID, Ven_BMP);
                            }

                        }

                        if (CheckFoundVen && CheckSentOTP == false)
                        {

                            SentOTP();
                        }

                        if (CheckSentOTP && CheckClickLogin == false)
                        {
                            SearchLogin(deviceID, Login_BMP);
                        }

                        if (CheckClickLogin && !CheckClickEmail)
                        {
                            SearchEmail(deviceID, Email_BMP, key.email, key.pass);
                            if (CheckClickEmail)
                            {
                                var count = 0;
                                var loop = true;
                                while (loop)
                                {
                                    if(count < 10)
                                    {
                                        var check = SearchError(deviceID, Error_BMP);
                                        if(check == true)
                                        {
                                            count = 10;
                                        }
                                        Thread.Sleep(1000);
                                        count++;

                                    }
                                    else
                                    {
                                        loop = false;
                                    }
                                  
                                }
                                  
                            }
                        }

                        if (CheckError)
                        {
                            SearchBagWallet(deviceID, BAG_BMP);
                            if (CheckClickBagWallet)
                            {
                                var chk = SearchDeleteWallet(deviceID, DeleteWallet_BMP);
                                if (chk == true)
                                {
                                    lineNotifyMessage("Email " + key.email + " link wallet fail!!");

                                    goto repeat2;
                                }
                            }
                        }

                        if (CheckClickEmail && !CheckError)
                        {
                            var loopDelete = true;
                            while (loopDelete)
                            {
                                if (!CheckClickIDLE)
                                {
                                    SearchIDLE(deviceID, IDLE_BMP);

                                }

                                SearchIconC2X(deviceID, ICONC2X_BMP);
                                SearchBagWallet(deviceID, BAG_BMP);
                                if (CheckClickBagWallet)
                                {
                                    var chk = SearchDeleteWallet(deviceID, DeleteWallet_BMP);
                                    if (chk == true)
                                    {
                                        lineNotifyMessage("Email " + key.email + " link wallet success");
                                        loopDelete = false;
                                        status = false;
                                    }
                                }
                               

                            }

                        }
                    }



                }

                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Swap account total success");


            }
            else
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Please check connect to devices");
            }
        }

        public static bool SearchDeleteWallet(string deviceID, Bitmap BMP)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var walletPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);

            if (walletPoint != null)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found delete wallet");
                KAutoHelper.ADBHelper.Tap(deviceID, walletPoint.Value.X, walletPoint.Value.Y);
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Click delete wallet");
                Thread.Sleep(1500);
                KAutoHelper.ADBHelper.Tap(deviceID, 754, 1437);
                return true;

            }
            else
            {
                return false;
            }
        }

        public static bool SearchError(string deviceID, Bitmap BMP)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var walletPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);

            if (walletPoint != null)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found error signature");

                KAutoHelper.ADBHelper.Tap(deviceID, 532, 1421);
                CheckError = true;
                return true;

            }
            else
            {
                return false;
            }
        }


        public static void SearchRecover(string deviceID, Bitmap BMP)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var walletPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);

            if (walletPoint != null)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found Recover");
                KAutoHelper.ADBHelper.Tap(deviceID, walletPoint.Value.X, walletPoint.Value.Y);
                CheckClickRecover = true;
            }
        }

        public static void SearchLogin(string deviceID, Bitmap BMP)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var walletPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);

            if (walletPoint != null)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found hive login");
                KAutoHelper.ADBHelper.Tap(deviceID, walletPoint.Value.X, walletPoint.Value.Y);

                CheckClickLogin = true;
            }
        }

        public static void SearchIconC2X(string deviceID, Bitmap BMP)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var walletPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);

            if (walletPoint != null)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found Icon C2X");
                OpenApp(deviceID, "c2xstation.android");


            }
        }

        public static void SearchBagWallet(string deviceID, Bitmap BMP)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var bagWalletPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);
            //var aaa = KAutoHelper.ImageScanOpenCV.Find(screen, BMP);
            //aaa.Save("bag.png");
            if (bagWalletPoint != null)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found bag wallet");
                
                KAutoHelper.ADBHelper.Tap(deviceID, bagWalletPoint.Value.X+40, bagWalletPoint.Value.Y+50);
                CheckClickBagWallet = true;


            }
        }

        public static void SearchIDLE(string deviceID, Bitmap BMP)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var backPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);

            if (backPoint != null)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found IDLE LUCA");
                KAutoHelper.ADBHelper.Tap(deviceID, backPoint.Value.X, backPoint.Value.Y);
                KillApp(deviceID, "c2xstation.android");
                CheckClickIDLE = true;


            }
        }

        public static void SearchEmail(string deviceID, Bitmap BMP,string email , string pass)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var walletPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);

            if (walletPoint != null)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found Email");
                KAutoHelper.ADBHelper.Tap(deviceID, walletPoint.Value.X, walletPoint.Value.Y);
                InputText(deviceID, email);
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Input email success");
                Thread.Sleep(500);
                KAutoHelper.ADBHelper.Tap(deviceID, 223, 1096);
                InputText(deviceID, pass);
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Input pass success");
                Thread.Sleep(500);
                KAutoHelper.ADBHelper.Tap(deviceID, 758, 719);
                Thread.Sleep(1000);
                KAutoHelper.ADBHelper.Tap(deviceID, 936, 1599);
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Click login");
                CheckClickEmail = true;
            }
        }

        public static void SearchOne(string deviceID, Bitmap BMP)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var onePoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);

            if (onePoint != null)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found +1");
                KAutoHelper.ADBHelper.Tap(deviceID, onePoint.Value.X, onePoint.Value.Y);
                CheckClickOne = true;


            }
        }

        public static void SearchVen(string deviceID, Bitmap BMP)
        {
            var loop = true;

            while (loop)
            {
                var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
                var onePoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);

                if (onePoint != null)
                {
                    Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found +84");
                    KAutoHelper.ADBHelper.Tap(deviceID, onePoint.Value.X+500, onePoint.Value.Y);
                    CheckFoundVen = true;
                    loop = false;
                }
                else
                {
                    KAutoHelper.ADBHelper.Swipe(deviceID, 536, 838, 536, 410);
                    
                }
            }
           
        }

        public static void cancleOTP(int id_giaodich)
        {
            string apikey = "550a9521-12fa-42ee-b5cc-7dc1b979792c";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient("http://api.codesim.net/api/CodeSim/HuyGiaoDich?apikey="+apikey+"&giaodich_id=" + id_giaodich);
            client.Timeout = 10000;
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            client.Timeout = 10000;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Cancle OTP success");
            }
        }

        public static string GetOTP(int id_giaodich)
        {
            var loop = true;
            var count = 0;
            var strOTP = "";
            while (loop)
            {
                string apikey = "550a9521-12fa-42ee-b5cc-7dc1b979792c";
                
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient("http://api.codesim.net/api/CodeSim/KiemTraGiaoDich?apikey=" + apikey + "&giaodich_id=" + id_giaodich);
                client.Timeout = 10000;
                var request = new RestRequest(Method.GET);
                var response = client.Execute(request);
                client.Timeout = 10000;
                ListSmModel listSm = new ListSmModel();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    listSm = Newtonsoft.Json.JsonConvert.DeserializeObject<ListSmModel>(response.Content);
                    if (listSm.stt == 1 && listSm.data.status == 0)
                    {
                        count = count + 1;
                        if(count >= 60)
                        {
                            loop = false;
                            cancleOTP(id_giaodich);
                        }
                        else
                        {
                            Thread.Sleep(1200);
                            continue;
                        }
                    }

                    if (listSm.stt == 1 && listSm.data.status == 1)
                    {

                        Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] You get OTP");

                        ListData listData = new ListData();
                        listData = listSm.data;

                        List<ListSm> listList = new List<ListSm>();

                        listList = listData.listSms;

                        foreach (var item in listList)
                        {
                            strOTP = GetSubstringByString("[","]", item.smsContent);
                            loop = false;
                        }
                        
                       

                    }


                }
            }
           

            return strOTP;
        }
        

        public static void SentOTP()
        {
            repeat:
            
                string apikey = "";
            
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient("http://api.codesim.net/api/CodeSim/DangKy_GiaoDich?apikey="+apikey+"&dichvu_id=133&so_sms_nhan=1");
                client.Timeout = 10000;
                var request = new RestRequest(Method.GET);
                
                var response = client.Execute(request);
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] With OTP ");
                
                OTPModel otp = new OTPModel();
                if (response.StatusCode == HttpStatusCode.OK)
                {

                    otp = Newtonsoft.Json.JsonConvert.DeserializeObject<OTPModel>(response.Content);
                    if(otp.stt == 1)
                    {
                        var phoneNumber = otp.data.phoneNumber;
                        var id_giaodich = otp.data.id_giaodich;
                        KAutoHelper.ADBHelper.Tap(deviceID, 385, 1282);
                        InputText(deviceID, phoneNumber);
                        Thread.Sleep(1000);
                        KAutoHelper.ADBHelper.Tap(deviceID, 480, 1143);
                        Thread.Sleep(1000);
                        KAutoHelper.ADBHelper.Tap(deviceID, 777, 1476);
                        Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Telphone: "+ phoneNumber+" wait OTP");
                        var strOTP = GetOTP(id_giaodich);
                        if(strOTP == "")
                        {
                            Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Not have OTP: ");
                            KAutoHelper.ADBHelper.Tap(deviceID, 274, 1220);
                            Thread.Sleep(1500);
                            SearchVen(deviceID, Ven_BMP);
                            goto repeat;
                        }else
                        {
                            Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] OTP: " + strOTP);
                            KAutoHelper.ADBHelper.Tap(deviceID, 290, 997);
                            InputText(deviceID, strOTP);
                            KAutoHelper.ADBHelper.Tap(deviceID, 760, 708);
                            Thread.Sleep(1500);
                            KAutoHelper.ADBHelper.Tap(deviceID, 767, 1221);
                            Thread.Sleep(1500);
                            KAutoHelper.ADBHelper.Tap(deviceID, 913, 1435);
                            CheckSentOTP = true;
                    }


                    }




                }

            
        }


        public static void SearchAccept(string deviceID, Bitmap BMP)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var agreePoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);

            if (agreePoint != null)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found agree to All");
                KAutoHelper.ADBHelper.Tap(deviceID, agreePoint.Value.X, agreePoint.Value.Y);
                Thread.Sleep(1000);
                KAutoHelper.ADBHelper.Tap(deviceID, 512, 2185);
                Thread.Sleep(1500);
                KAutoHelper.ADBHelper.Tap(deviceID, 545, 1322);
                CheckClickAgree = true;
            }
        }




        public static void SearchExplore(string deviceID, Bitmap BMP)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var walletPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);

            if (walletPoint != null)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found Explore");
                KAutoHelper.ADBHelper.Tap(deviceID, walletPoint.Value.X, walletPoint.Value.Y);
                CheckClickExploer = true;

            }
        }

        public static bool SearchTextConvert(string deviceID, Bitmap BMP)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var iconGamePoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);
            //var aaa = KAutoHelper.ImageScanOpenCV.Find(screen, BMP);
            //aaa.Save("ggg.png");
            if (iconGamePoint != null)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found text convert");
                return true;
            }
            else
            {
                return false;
            }
           
        }



        public static void SearchConvert(string deviceID, Bitmap BMP)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var convertPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);
            if (convertPoint != null)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found Covert");
                KAutoHelper.ADBHelper.Tap(deviceID, convertPoint.Value.X, convertPoint.Value.Y);
                CheckClickConvert = true;

            }
        }

        public static void SearchIconGame(string deviceID, Bitmap BMP)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var iconGamePoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);
            //var aaa = KAutoHelper.ImageScanOpenCV.Find(screen, BMP);
            //aaa.Save("bbb.png");
            if (iconGamePoint != null)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found icon game");
                KAutoHelper.ADBHelper.Tap(deviceID, iconGamePoint.Value.X, iconGamePoint.Value.Y);

                CheckClickIconGame = true;
            }
        }


        public static void SearchUseSeed(string deviceID, Bitmap BMP)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var walletPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);
            if (walletPoint != null)
            {

                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found UseSeed");
                KAutoHelper.ADBHelper.Tap(deviceID, walletPoint.Value.X, walletPoint.Value.Y);

            }
        }

        public static void SearchWalletName(string deviceID, Bitmap BMP, string name)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var walletPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);
            ;
            if (walletPoint != null)
            {
                string input = name;
                string[] values = input.Split('@');
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found Input Wallet Name");
                KAutoHelper.ADBHelper.Tap(deviceID, 389, 537);
                Thread.Sleep(1000);
                var inputText = InputText(deviceID, values[0]);
                if (inputText != null)
                {
                    Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Input wallet name success");
                    KAutoHelper.ADBHelper.Tap(deviceID, 242, 818);
                    var password = InputText(deviceID, "qwertyuiop");
                    if (password != null)
                    {
                        Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Input password success");
                        KAutoHelper.ADBHelper.Tap(deviceID, 298, 1072);
                        var confirmPassword = InputText(deviceID, "qwertyuiop");
                        if (confirmPassword != null)
                        {
                            Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Input confirm password success");
                            KAutoHelper.ADBHelper.Tap(deviceID, 326, 1215);
                            CheckInputNameWallet = true;
                        }
                    }
                }

            }
        }

        public static void SearchPaste(string deviceID, Bitmap BMP, string mneni)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var walletPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);

            if (walletPoint != null)
            {

                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Found Paste");
                KAutoHelper.ADBHelper.Tap(deviceID, 536, 628);
                Thread.Sleep(1500);
                var test = InputText(deviceID, mneni);
                if (test != null)
                {
                    Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Input mninome success");
                    Thread.Sleep(1000);
                    KAutoHelper.ADBHelper.Tap(deviceID, 525, 918);
                    CheckInputMneni = true;
                }

            }

        }

        public static bool SearchNext(string deviceID, Bitmap BMP)
        {
            var screen = KAutoHelper.ADBHelper.ScreenShoot(deviceID);
            var NextPoint = KAutoHelper.ImageScanOpenCV.FindOutPoint(screen, BMP);

            if (NextPoint != null)
            {
                KAutoHelper.ADBHelper.Tap(deviceID, NextPoint.Value.X, NextPoint.Value.Y);
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Click Next");
                return true;
            }
            else
            {
                return false;
            }

        }

        public static string InputText(string deviceID, string text)
        {
            string cmdCommand = string.Format(INPUT_TEXT_DEVICES, deviceID, text.Replace(" ", "%s").Replace("&", "\\&").Replace("<", "\\<")
                .Replace(">", "\\>")
                .Replace("?", "\\?")
                .Replace(":", "\\:")
                .Replace("{", "\\{")
                .Replace("}", "\\}")
                .Replace("[", "\\[")
                .Replace("]", "\\]")
                .Replace("|", "\\|"));
            string text2 = ExecuteCMD(cmdCommand);
            return text2;
        }

        public static void OpenApp(string deviceID, string app)
        {
            string cmdCommand = string.Format(OPEN_APP_DEVICES, deviceID, app);
            string text2 = ExecuteCMD(cmdCommand);
        }

        public static void KillApp(string deviceID, string app)
        {
            string cmdCommand = string.Format(Kill_APP_DEVICES, deviceID, app);
            string text2 = ExecuteCMD(cmdCommand);
        }
        public static string ExecuteCMD(string cmdCommand)
        {
            try
            {
                Process process = new Process();
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.WorkingDirectory = ADB_FOLDER_PATH;
                processStartInfo.FileName = "cmd.exe";
                processStartInfo.CreateNoWindow = true;
                processStartInfo.UseShellExecute = false;
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.RedirectStandardInput = true;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.Verb = "runas";
                process.StartInfo = processStartInfo;
                process.Start();
                process.StandardInput.WriteLine(cmdCommand);
                process.StandardInput.Flush();
                process.StandardInput.Close();
                process.WaitForExit();
                return process.StandardOutput.ReadToEnd();
            }
            catch
            {
                return null;
            }
        }
    }
}

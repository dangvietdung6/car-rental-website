using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Firebase.Database;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace ThueXeMay.Helpers
{
    public class FirebaseHelper
    {
        private static readonly IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "AIzaSyBy893jXsqDCWwDTxKgVEnwejZAs00nvlM", // Thay bằng Secret Key của bạn
            BasePath = "https://thuexe-bfde0-default-rtdb.firebaseio.com/" // Thay bằng URL của Firebase Database
        };
        private static readonly IFirebaseClient client = new FireSharp.FirebaseClient(config);

        public async Task SetDataAsync(string path, object data)
        {
            try
            {
                await client.SetAsync(path, data); // Chỉ cần thực hiện lệnh Set, không cần lấy "Result"
                Console.WriteLine("Dữ liệu đã lưu thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
            }
        }

        public static async Task<T> GetDataAsync<T>(string path)
        {
            try
            {
                FirebaseResponse response = await client.GetAsync(path);
                return response.ResultAs<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy dữ liệu: " + ex.Message);
                return default(T);
            }
        }

        public async Task<List<T>> GetAllDataAsync<T>(string path)
        {
            try
            {
                FirebaseResponse response = await client.GetAsync(path);
                return response.ResultAs<List<T>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy dữ liệu: " + ex.Message);
                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using AnimusTest.Models;
using Formatting = Newtonsoft.Json.Formatting;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Forms;
using System.Net.Http;
using System.Drawing;
using SkiaSharp;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Xabe.FFmpeg;
using Microsoft.Win32;

namespace AnimusTest.Controls
{
    public class FileController
    {
        private string fileExtension = "aiau";
        public FileController() { }

        public Timeline OpenFile()
        {

            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "моя онямація";
            dialog.DefaultExt = $".{fileExtension}";
            dialog.Filter = $"Text documents (.{fileExtension})|*.{fileExtension}";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                string json = File.ReadAllText(filename);
                return JsonConvert.DeserializeObject<Timeline>(json);
            }
            else
            {
                MessageBox.Show("Could not open project");
                return null;
            }


        }

        public static async Task<bool> PublishProjectAsync(Project project)
        {

            HttpClient httpClient = new HttpClient();

            // ПОКИ ЩО
            // ТЕСТОВО
            // ПУБЛІКУЄМО ЯК КАРТИНКУ
            // а потім додати вікно вибору формату


            string token = File.Exists("E://AUTHTOKEN.txt")
                ? File.ReadAllText("E://AUTHTOKEN.txt")
                : null;

            if (string.IsNullOrEmpty(token))
            {
                MessageBox.Show("Користувач не авторизований.");
                return false;
            }


            //токен до заголовка
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            //string json = JsonConvert.SerializeObject(project);
            string json = JsonConvert.SerializeObject(ExportProject(project), Formatting.Indented);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PostAsync("http://localhost:3000/publish", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Проєкт успішно опубліковано!");
                    return true;
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Сталася помилка: {response.StatusCode}\n{error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ексепшіон: {ex.Message}");

                return false;
            }
        }


        public static ProjectSerializable ExportProject(Project project)
        {
            return new ProjectSerializable
            {
                title = project.Title,
                description = project.Description,
                publishedAt = DateOnly.FromDateTime(DateTime.Now),
                renderedImage = FileController.EncodeLayersToBase64(project.Frames[0].Layers, project.Width, project.Height)
            };
        }

        public static SKBitmap LoadBitmap(string path)
        {
            using var stream = File.OpenRead(path);
            return SKBitmap.Decode(stream);
        }

        public static void SaveCurrentCanvasToFile(SKSurface surface, string path)
        {


            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = File.OpenWrite(path);
            data.SaveTo(stream);

            surface.Dispose(); //звільнити ресурс

            MessageBox.Show("Експорт успішний!");
        }

        public static void SaveProjectToProjectFile(Project project, string path) // тут записую проект в бінарний файл, структура даних буде в звіті
        {
            using var stream = File.Open(path, FileMode.Create);
            using var writer = new BinaryWriter(stream);

            writer.Write(project.Title ?? "");
            writer.Write(project.Description ?? "");
            writer.Write(project.Author ?? "");
            writer.Write(project.CreatedAt.ToBinary());
            writer.Write(project.LastModifiedAt.ToBinary());

            writer.Write(project.Width);
            writer.Write(project.Height);
            writer.Write(project.Frames.Count);

            foreach (var frame in project.Frames)
            {
                writer.Write(frame.Layers.Count);
                foreach (var layer in frame.Layers)
                {
                    writer.Write(layer.IsVisible);
                    writer.Write(layer.Opacity);

                    // bitmap як PNG у пам’ять
                    using var image = layer.Bitmap.Encode(SKEncodedImageFormat.Png, 100); //100 - збереження без втрати якості
                    var bytes = image.ToArray();

                    writer.Write(bytes.Length);
                    writer.Write(bytes);
                }
            }
        }

        // ПЕРЕВІРИТИ ЧИ СПІВПАДАЄ СТРУКТУРА ДАНИХ ПРИ ЗАПИСУ І ЧИТАННІ!!!

        public static Project LoadProjectFromProjectFile(string path)
        {
            using var stream = File.OpenRead(path);
            using var reader = new BinaryReader(stream);

            var project = new Project
            {
                Title = reader.ReadString(),
                Description = reader.ReadString(),
                Author = reader.ReadString(),
                CreatedAt = DateTime.FromBinary(reader.ReadInt64()),
                LastModifiedAt = DateTime.FromBinary(reader.ReadInt64())
            };

            var width = reader.ReadInt32();
            var height = reader.ReadInt32();
            var frameCount = reader.ReadInt32();

            for (int i = 0; i < frameCount; i++)
            {
                var frame = new Frame();
                var layerCount = reader.ReadInt32();

                for (int j = 0; j < layerCount; j++)
                {
                    var isVisible = reader.ReadBoolean();
                    var opacity = reader.ReadSingle();

                    var layerBitmapLength = reader.ReadInt32();
                    var bytes = reader.ReadBytes(layerBitmapLength);

                    var bitmap = SKBitmap.Decode(bytes);
                    var layer = new Layer(bitmap)
                    {
                        IsVisible = isVisible,
                        Opacity = opacity
                    };
                    frame.Layers.Add(layer);
                }

                project.Frames.Add(frame);
            }

            return project;

        }


        public static void ExportToSeriesOfImages(Project project, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            Models.Frame frame = project.Frames[0];
            for (int i = 0; i < project.Frames.Count; i++)
            {
                /*
                if (project.Frames[i].isDirty) // якщо у кадрі щось намальовано - то ми його рендеримо, якщо ні - то рендериться попередній
                {
                    frame = project.Frames[i];
                }
                */
                frame = project.Frames[i];
                var filePath = Path.Combine(directoryPath, $"frame{(i + 1):D3}.png");
                using var surface = SKSurface.Create(new SKImageInfo(1500, 1000));
                using var canvas = surface.Canvas;

                canvas.Clear(SKColors.White); //для того, щоб не було чорного заднього фону при рендерингу відео

                foreach (var layer in frame.Layers)
                {
                    if (!layer.IsVisible || layer.Bitmap == null) continue;
                    canvas.DrawBitmap(layer.Bitmap, SKPoint.Empty);
                }
                using var image = surface.Snapshot();
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);
                using var stream = File.OpenWrite(filePath);
                data.SaveTo(stream);
            }
            MessageBox.Show("Successfully exported to series of images!");
        }

        public static async void SaveProjectAsAnimationMovie(Project project)
        {
            
            string directoryPath = @"E:\Animatus\Output\" + project.Title;
            ;
            ExportToSeriesOfImages(project, directoryPath);

            await ConvertFromSeriesToVideo(project.Title, directoryPath);

            MessageBox.Show("Експорт серії зображень успішний!");

        }

        public static async Task ConvertFromSeriesToVideo(string projectTitle, string path)
        {
            FFmpeg.SetExecutablesPath(@"E:\ffmpeg\bin");

            var frameRate = 24; //fps
            var imagePattern = Path.Combine(path, "frame%03d.png");

            if (!File.Exists(Path.Combine(path, "frame001.png")))
            {
                Console.WriteLine("");
                return;
            }
            //string outputPath = @"E:\Animations_Output\animation.mp4";
            string outputPath = @"E:\Animations_Output\" + projectTitle +".mp4";

            var conversion = FFmpeg.Conversions.New()
                .AddParameter($"-framerate {frameRate}")
                .AddParameter($"-i \"{imagePattern}\"") 
                .AddParameter("-c:v libx264") 
                .AddParameter("-pix_fmt yuv420p")
                .SetOutput(outputPath)
                .SetOverwriteOutput(true);

            conversion.SetOverwriteOutput(true);

            Console.WriteLine("Старт конвертації...");
            await conversion.Start();
            Console.WriteLine("Відео збережено у: " + outputPath); 

        }


        public static string EncodeLayersToBase64(List<Layer> projectLayers, int width, int height)
        {
            using var merged = new SKBitmap(width, height);
            using var canvas = new SKCanvas(merged);
            canvas.Clear(SKColors.White);

            foreach (var layer in projectLayers)
            {
                if (layer != null)
                    canvas.DrawBitmap(layer.Bitmap, SKPoint.Empty);
            }

            using var image = SKImage.FromBitmap(merged);
            using var data = image.Encode(SKEncodedImageFormat.Png, 60);
            string base64 = Convert.ToBase64String(data.ToArray());
            return $"data:image/png;base64,{base64}";
        }

    }

}
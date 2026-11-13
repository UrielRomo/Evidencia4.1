using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Evidencia4._1
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

       
        public class RegistroPerfume
        {
            public DateTime Fecha { get; set; }
            public string AromaSeleccionado { get; set; }
            public string EventoSeleccionado { get; set; }
            public string MomentoSeleccionado { get; set; }
            public List<string> PerfumesRecomendados { get; set; }
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
          
            if (CmbAroma.SelectedItem == null || CmbEvento.SelectedItem == null || CmbMomento.SelectedItem == null)
            {
                InfoBarGuardado.IsOpen = false;
                TxtResumenPerfil.Text = "Por favor responde todas las preguntas para darte una recomendación precisa.";
                TxtRecomendaciones.Text = "";
                BorderResultado.Visibility = Visibility.Visible;
                return;
            }

            string aroma = CmbAroma.SelectedItem.ToString();
            string evento = CmbEvento.SelectedItem.ToString();
            string momento = CmbMomento.SelectedItem.ToString();

            
            List<string> perfumes = ObtenerPerfumesSegunCriterios(aroma, evento, momento);

            
            TxtResumenPerfil.Text = $"Perfil: Aroma {aroma.Split(' ')[0]} | Ocasión: {evento} | Momento: {momento.Split(' ')[0]}";

            string resultadoTexto = "";
            for (int i = 0; i < perfumes.Count; i++)
            {
                resultadoTexto += $"{i + 1}. {perfumes[i]}\n\n";
            }

            TxtRecomendaciones.Text = resultadoTexto.Trim();
            BorderResultado.Visibility = Visibility.Visible;

            
            InfoBarGuardado.IsOpen = true;

            
            try
            {
                
                string carpetaDescargas = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                string rutaArchivo = Path.Combine(carpetaDescargas, "HistorialPerfumes.json");

               
                var nuevoRegistro = new RegistroPerfume
                {
                    Fecha = DateTime.Now,
                    AromaSeleccionado = aroma,
                    EventoSeleccionado = evento,
                    MomentoSeleccionado = momento,
                    PerfumesRecomendados = perfumes
                };

               
                List<RegistroPerfume> listaRegistros = new List<RegistroPerfume>();
                if (File.Exists(rutaArchivo))
                {
                    string jsonExistente = File.ReadAllText(rutaArchivo);
                    if (!string.IsNullOrWhiteSpace(jsonExistente))
                    {
                        var registrosPrevios = JsonSerializer.Deserialize<List<RegistroPerfume>>(jsonExistente);
                        if (registrosPrevios != null)
                        {
                            listaRegistros = registrosPrevios;
                        }
                    }
                }

               
                listaRegistros.Add(nuevoRegistro);

               
                var opcionesJson = new JsonSerializerOptions { WriteIndented = true };
                string jsonActualizado = JsonSerializer.Serialize(listaRegistros, opcionesJson);


                File.WriteAllText(rutaArchivo, jsonActualizado);
            }
            catch (Exception)
            {
                InfoBarGuardado.Title = "Aviso";
                InfoBarGuardado.Message = "Tus respuestas se mostraron, pero hubo un problema al guardarlas en la carpeta Descargas.";
                InfoBarGuardado.Severity = InfoBarSeverity.Warning;
            }
        }

        private List<string> ObtenerPerfumesSegunCriterios(string aroma, string evento, string momento)
        {
            List<string> lista = new List<string>();

            if (aroma.Contains("Amaderado"))
            {
                lista.Add("Dior Homme Intense – Elegante, maderas profundas con un toque de iris.");
                lista.Add("Terre d'Hermès – Un clásico amaderado mineral, perfecto para la formalidad.");
                lista.Add("Tom Ford Oud Wood – Exclusivo, lujoso y con un carácter amaderado especiado fuerte.");
                lista.Add("Bleu de Chanel (Eau de Parfum) – Maderas ambarinas muy versátiles y sofisticadas.");
                lista.Add("Montblanc Explorer – Maderas modernas ideales para el día a día.");
            }
            else if (aroma.Contains("Cítrico"))
            {
                lista.Add("Acqua di Giò (Giorgio Armani) – El rey de los cítricos acuáticos, fresco y limpio.");
                lista.Add("Chanel Allure Homme Sport – Cítrico vigoroso con un fondo cremoso y almizclado.");
                lista.Add("Dolce & Gabbana Light Blue – Sumamente fresco, ideal para climas cálidos y diurnos.");
                lista.Add("Versace Man Eau Fraiche – Cítrico amaderado muy veraniego y casual.");
                lista.Add("Mandarino di Amalfi (Tom Ford) – Cítrico luminoso de alta gama con notas herbales.");
            }
            else if (aroma.Contains("Oriental"))
            {
                lista.Add("Yves Saint Laurent La Nuit de l'Homme – Dulce, especiado y altamente seductor para la noche.");
                lista.Add("Jean Paul Gaultier Ultra Male – Intenso, avainillado y perfecto para fiestas.");
                lista.Add("Spicebomb Extreme (Viktor&Rolf) – Especias cálidas con tabaco y vainilla muy potente.");
                lista.Add("Armani Code Parfum – Notas orientales ambarinas profundas y modernas.");
                lista.Add("Paco Rabanne One Million – Dulce, especiado, llamativo y de gran proyección.");
            }
            else 
            {
                lista.Add("Nautica Voyage – Económico, sumamente fresco y limpio para uso diario.");
                lista.Add("Versace Dylan Blue – Fresco con un toque amaderado e incienso muy versátil.");
                lista.Add("Acqua di Giò Profondo – Notas marinas profundas con un toque mineral fresco.");
                lista.Add("Davidoff Cool Water – El clásico fresco y aromático por excelencia.");
                lista.Add("Polo Blue (Ralph Lauren) – Frescura elegante con melón y pepino sobre fondo amaderado.");
            }

            return lista;
        }
    }
}
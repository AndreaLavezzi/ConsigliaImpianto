using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace prova_ingresso
{
    public partial class Form1 : Form
    {
        // Variabili globali ottenute dall'inserimento dell'utente nel form.
        public static double costo_gas = 0, costo_elettricità = 0, consumo_gas = 0, consumo_elettricità = 0;
        
        public static int anni;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void vai_BTN_Click(object sender, EventArgs e)
        {
            bool errore;
            try
            {
                // Viene provata la conversione dei valori inseriti dall'utente, se nessuna conversione interrompe il try la variabile "errore" viene settata a false.
                consumo_elettricità = double.Parse(consumo_elettrico_TXT.Text);
                consumo_gas = double.Parse(consumo_gas_TXT.Text);
                costo_elettricità = double.Parse(costo_elettricità_TXT.Text);
                costo_gas = double.Parse(costo_gas_TXT.Text);
                anni = Int32.Parse(anni_TXT.Text);
                if(consumo_elettricità < 0 || consumo_gas < 0 || costo_elettricità < 0 || costo_gas < 0 || anni < 0)
                {
                    throw new Exception();
                }
                errore = false;
            }
            catch
            {
                // Se viene catturato un errore la variabile errore viene settata a true.
                errore = true;
                MessageBox.Show("Inserimento non valido. Inserire solamente numeri positivi e virgole.");
            }

            

            if (!errore)
            {
                Trova_Impianto_Migliore();
            }
        }

        private void Trova_Impianto_Migliore()
        {
            // Percorso del file json che contiene la lista di impianti.
            string percorso_impianti = "impianti/impianti.json";

            // Viene trovato il bottone cliccato che corrisponde all'impianto in uso.
            var checked_button = this.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);

            // Si popola la lista di impianti deserializzando il json che li contiene.
            string json = File.ReadAllText(percorso_impianti);
            List<Impianto> impianti = JsonConvert.DeserializeObject<List<Impianto>>(json);
            
            // Lista di bollette ipotetiche per ogni impianto.
            List<Bolletta> bollette = new List<Bolletta>();


            foreach (Impianto impianto in impianti)
            {
                double consumo_principale = 0, consumo_secondario = 0, costo_annuale = 0, spesa_installazione = impianto.Costo_Installazione, spesa_materia_gas;
                Bolletta bolletta;

                switch (impianto.Tipo_Energia)
                {
                    case "Gas":
                        consumo_principale = consumo_gas;
                        consumo_secondario = consumo_elettricità;
                        costo_annuale = costo_gas;
                        break;
                    case "Elettrico":
                        consumo_principale = consumo_elettricità;
                        consumo_secondario = consumo_gas;
                        costo_annuale = costo_elettricità;
                        break;
                }

                // Se l'impianto corrispondente corrisponde all'impianto selezionato non si conta la spesa di installazione.
                if(checked_button.Text == impianto.Nome)
                {
                    spesa_installazione = 0;
                }

                // Si calcola la spesa del gas in base ai consumi inseriti.
                spesa_materia_gas = impianto.Costo_Totale(costo_annuale, consumo_principale, consumo_secondario);

                // Si crea una nuova bolletta cche tiene conto della spesa per tanti anni quanti ne ha decisi l'utente.
                bolletta = new Bolletta(spesa_materia_gas, spesa_installazione, impianto.Nome, anni);

                bollette.Add(bolletta);
            }

            // Viene ordinata la lista di bollette in base alla spesa totale.
            bollette = bollette.OrderBy(b => b.Spesa_Totale).ToList();

            // Si controlla se l'impianto della bolletta meno costosa corrisponde all'impianto selezionato.
            if (bollette[0].Impianto == checked_button.Text)
            {
                MessageBox.Show($"L'impianto attuale è quello migliore per i prossimi {anni} anni");
            }
            else
            {
                MessageBox.Show($"L'impianto migliore è {bollette[0].Impianto} dal costo di {Math.Round(bollette[0].Spesa_Totale, 2)}");
            }
        }

    }
}
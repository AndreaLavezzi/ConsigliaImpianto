using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prova_ingresso
{
    internal class Bolletta
    {
        double spesa_materia_gas, spesa_trasporto_gestione_contatore, spesa_oneri_sistema, spesa_fissa_gas, spesa_totale, spesa_installazione_impianto;
        string impianto;

        public string Impianto { get { return impianto; } set { impianto = value; } }
        public double Spesa_Totale { get { return spesa_totale; } }
        public Bolletta(double spesa_materia_gas, double spesa_installazione_impianto, string impianto, int anni)
        {
            this.spesa_trasporto_gestione_contatore = 96;
            this.spesa_oneri_sistema = 47;
            this.spesa_fissa_gas = 70;
            this.spesa_materia_gas = spesa_materia_gas;
            this.spesa_installazione_impianto = spesa_installazione_impianto;
            this.impianto = impianto;
            this.spesa_totale = Get_Spesa_Totale(anni);
        }

        // La spesa totale della bolletta è pari alla somma dei costi fissi annui più il costo annuo del gas moltiplicata per gli anni che si tengono in considerazione.
        // Si aggiunge alla fine la spesa per l'installazione dell'impianto nel caso non sia quello in uso.
        private double Get_Spesa_Totale(int anni)
        {
            double spesa = spesa_fissa_gas + spesa_oneri_sistema + spesa_trasporto_gestione_contatore + spesa_materia_gas;
            spesa *= anni;
            spesa += spesa_installazione_impianto;
            return spesa;
        }
    }
}

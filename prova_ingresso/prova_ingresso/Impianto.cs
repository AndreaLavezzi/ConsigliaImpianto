using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace prova_ingresso
{
    internal class Impianto
    {
        private string nome, tipo_energia;
        private double rendimento, costo_installazione;
        public double Costo_Installazione { get { return costo_installazione; } set { costo_installazione = value; } }
        public string Tipo_Energia { get { return tipo_energia; } set { tipo_energia = value; } }
        public string Nome { get { return nome; } set { nome = value; } }
        public double Rendimento { get { return rendimento; } set { rendimento = value; } }


        public Impianto(double rendimento, double costo_installazione, string tipo_energia, string nome)
        {
            this.rendimento = rendimento;
            this.costo_installazione = costo_installazione;
            this.tipo_energia = tipo_energia;
            this.nome = nome;
        }

        public double Costo_Totale(double costo_energia, double consumo_principale, double consumo_secondario)
        {
            // Il costo totale è pari al costo dell'energia per kwh/smc moltiplicato per il consumo totale annuo dell'impianto
            double totale = costo_energia * Get_Consumo_Totale(consumo_secondario, consumo_principale);
            return totale;
        }

        public double Get_Consumo_Totale(double consumo_secondario, double consumo_principale)
        {
            double consumo_totale = 0;
            switch (tipo_energia)
            {
                // Se il tipo di energia usato dall'impianto è gas viene convertito il consumo secondario da kwh a smc
                case "Gas":
                    consumo_totale = consumo_secondario / (10.7 * rendimento);
                    break;
                // Se il tipo di energia usato dall'impianto è l'elettricità viene convertito il consumo da smc a kwh
                case "Elettrico":
                    consumo_totale = (consumo_secondario * 10.7) / rendimento; 
                    break;
            }
            // Si aggiunge il consumo principale
            consumo_totale += consumo_principale;
            return consumo_totale;
        }

    }
}

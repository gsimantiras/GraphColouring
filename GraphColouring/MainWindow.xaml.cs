using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphColouring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ArrayList graphs = new ArrayList();

        private Color[] myColors = new Color[] { Colors.Red, Colors.Blue, Colors.Green, Colors.Yellow };
        private String[] myColorStrings = new String[] { "red", "blue", "green", "yellow" };
        private int[] myColorId = new int[] { 0, 1, 2, 3};
        private string parent_1 = "";
        private string parent_2 = "";
        private string child_1 = "";
        private string child_2 = "";
        private int population_start = 50;

        System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
        private Dictionary<string, int> population = new Dictionary<string, int>();
        private List<KeyValuePair<string, int>> rankedPopulationList;

        public int globalID;
        private int colorsToUse = 4;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        Random rnd = new Random();
        public int previousErrors;
        public int newErrors;
        
        public MainWindow()
        {
            InitializeComponent();

            InitializeGraph();
            
            //populate();
            //setColors(rankedPopulationList[0].Key);

            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);

            //Run();

        }

        private void Run()
        {
            populate();
            SortByErrors();
            while (rankedPopulationList[0].Value != 0) {
                reNewPopulation();
                Mutates();
                SortByErrors();
                parent_1 = getFittedParent();
                parent_2 = getFittedParent();
                string[] children = CrossOver(parent_1, parent_2);
                child_1 = children[0];
                child_2 = children[1];
                child_1 = Mutate(child_1);
                child_2 = Mutate(child_2);
                if (!population.ContainsKey(child_1))
                {
                    population.Add(child_1, 100);
                }
                if (!population.ContainsKey(child_2))
                {
                    population.Add(child_2, 100);
                }
                SortByErrors();
                if (population[child_1] <= rankedPopulationList[0].Value)
                {
                    Console.WriteLine(child_1 + " - " + population[child_1]);
                }
                if (population[child_2] <= rankedPopulationList[0].Value)
                {
                    Console.WriteLine(child_2 + " - " + population[child_2]);
                }
                Console.WriteLine("population: " + population.Count);

                setColors(child_1);
            }
           


            if (rankedPopulationList[0].Value == 0)
            {
                setColors(rankedPopulationList[0].Key);
                dispatcherTimer.Stop();
                sw.Stop();
                Console.Write(gettime());
            }
            text.Text = rankedPopulationList[0].Key + " errors " + population[rankedPopulationList[0].Key] + " Time Passed:" + gettime().Substring(0, 5) + " Sec.";
        }

        private void reNewPopulation()
        {
            if (population.Count > population_start * 1.5)
            {
                for (int i = 0; i < population_start / 2; i++)
                {
                    string generated_cromosome = "";
                    for (int y = 0; y < graphs.Count; y++)
                    {
                        int rand = rnd.Next(colorsToUse);
                        generated_cromosome += rand + "";
                    }
                    population.Remove(rankedPopulationList[rankedPopulationList.Count - 1 - i].Key);
                    if (!population.ContainsKey(generated_cromosome)) {
                        population.Add(generated_cromosome, 100);
                    }
                }
                population_start = population.Count;
            }
        }

        private string[] CrossOver(string parent_1, string parent_2)
        {
            int crossPoint = rnd.Next(parent_1.Length);
            string dna_sample_p1_1 = parent_1.Substring(0,crossPoint);
            string dna_sample_p1_2 = parent_1.Substring(crossPoint, parent_1.Length - crossPoint);
            string dna_sample_p2_1 = parent_2.Substring(0, crossPoint);
            string dna_sample_p2_2 = parent_2.Substring(crossPoint, parent_2.Length - crossPoint);
            string[] children = new string[]{ dna_sample_p1_1 + dna_sample_p2_2, dna_sample_p2_1 + dna_sample_p1_2};
            return children;
        }

        private void InitializeGraph()
        {
            GraphMap n1 = new GraphMap(1);
            n1.chlidren.Add(2);
            n1.chlidren.Add(3);
            n1.chlidren.Add(4);
            n1.chlidren.Add(13);
            n1.chlidren.Add(15);
            n1.chlidren.Add(16);

            GraphMap n2 = new GraphMap(2);
            n2.chlidren.Add(1);
            n2.chlidren.Add(3);
            n2.chlidren.Add(5);
            n2.chlidren.Add(8);
            n2.chlidren.Add(9);
            n2.chlidren.Add(14);
            n2.chlidren.Add(15);
            n2.chlidren.Add(16);

            GraphMap n3 = new GraphMap(3);
            n3.chlidren.Add(1);
            n3.chlidren.Add(2);
            n3.chlidren.Add(4);
            n3.chlidren.Add(5);
            n3.chlidren.Add(6);

            GraphMap n4 = new GraphMap(4);
            n4.chlidren.Add(1);
            n4.chlidren.Add(3);
            n4.chlidren.Add(6);
            n4.chlidren.Add(13);

            GraphMap n5 = new GraphMap(5);
            n5.chlidren.Add(2);
            n5.chlidren.Add(3);
            n5.chlidren.Add(6);
            n5.chlidren.Add(7);
            n5.chlidren.Add(9);
            n5.chlidren.Add(10);

            GraphMap n6 = new GraphMap(6);
            n6.chlidren.Add(3);
            n6.chlidren.Add(4);
            n6.chlidren.Add(5);
            n6.chlidren.Add(7);
            n6.chlidren.Add(11);
            n6.chlidren.Add(13);

            GraphMap n7 = new GraphMap(7);
            n7.chlidren.Add(5);
            n7.chlidren.Add(6);
            n7.chlidren.Add(10);
            n7.chlidren.Add(11);

            GraphMap n8 = new GraphMap(8);
            n8.chlidren.Add(2);
            n8.chlidren.Add(9);
            n8.chlidren.Add(14);

            GraphMap n9 = new GraphMap(9);
            n9.chlidren.Add(2);
            n9.chlidren.Add(5);
            n9.chlidren.Add(8);
            n9.chlidren.Add(10);
            n9.chlidren.Add(12);
            n9.chlidren.Add(14);

            GraphMap n10 = new GraphMap(10);
            n10.chlidren.Add(5);
            n10.chlidren.Add(7);
            n10.chlidren.Add(9);
            n10.chlidren.Add(11);
            n10.chlidren.Add(12);

            GraphMap n11 = new GraphMap(11);
            n11.chlidren.Add(6);
            n11.chlidren.Add(7);
            n11.chlidren.Add(10);
            n11.chlidren.Add(12);
            n11.chlidren.Add(13);

            GraphMap n12 = new GraphMap(12);
            n12.chlidren.Add(9);
            n12.chlidren.Add(10);
            n12.chlidren.Add(11);
            n12.chlidren.Add(13);
            n12.chlidren.Add(14);
            n12.chlidren.Add(15);

            GraphMap n13 = new GraphMap(13);
            n13.chlidren.Add(1);
            n13.chlidren.Add(4);
            n13.chlidren.Add(6);
            n13.chlidren.Add(11);
            n13.chlidren.Add(12);
            n13.chlidren.Add(15);

            GraphMap n14 = new GraphMap(14);
            n14.chlidren.Add(2);
            n14.chlidren.Add(8);
            n14.chlidren.Add(9);
            n14.chlidren.Add(12);
            n14.chlidren.Add(15);

            GraphMap n15 = new GraphMap(15);
            n15.chlidren.Add(1);
            n15.chlidren.Add(2);
            n15.chlidren.Add(12);
            n15.chlidren.Add(13);
            n15.chlidren.Add(14);
            n15.chlidren.Add(16);

            GraphMap n16 = new GraphMap(16);
            n16.chlidren.Add(1);
            n16.chlidren.Add(2);
            n16.chlidren.Add(15);


            graphs.Add(n1);
            graphs.Add(n2);
            graphs.Add(n3);
            graphs.Add(n4);
            graphs.Add(n5);
            graphs.Add(n6);
            graphs.Add(n7);
            graphs.Add(n8);
            graphs.Add(n9);
            graphs.Add(n10);
            graphs.Add(n11);
            graphs.Add(n12);
            graphs.Add(n13);
            graphs.Add(n14);
            graphs.Add(n15);
            graphs.Add(n16);

        }

        private void Mutates()
        {
            for (int i = 0; i < rankedPopulationList.Count/10; i++)
            {
                int rand = rnd.Next(rankedPopulationList.Count);
                while (rankedPopulationList[rand].Value < 2) {
                    rand = rnd.Next(rankedPopulationList.Count);
                }
                string chromosomeToBeMutated = rankedPopulationList[rand].Key;
                string mutatedChromosome = RandomMutate(chromosomeToBeMutated);
                if (!population.ContainsKey(mutatedChromosome)) {
                    population.Remove(chromosomeToBeMutated);
                    population.Add(mutatedChromosome, 100); 
                }
            }
        }

        private string RandomMutate(string chromosomeToBeMutated)
        {
            int digitToChange = rnd.Next(chromosomeToBeMutated.Length);
            string changedChromosome="";
            for (int i = 0; i < chromosomeToBeMutated.Length; i++)
			{
                if (i != digitToChange){
                    changedChromosome += chromosomeToBeMutated.Substring(i, 1);
                }
                else {
                    int newDigit =rnd.Next(colorsToUse);
                    while (newDigit.Equals(chromosomeToBeMutated.Substring(i,1))){
                        newDigit = rnd.Next(colorsToUse);
                    }
                    changedChromosome += newDigit;
                }
			}
            return changedChromosome;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            reNewPopulation();
            Mutates();
            SortByErrors();
            parent_1 = getFittedParent();
            parent_2 = getFittedParent();
            string[] children = CrossOver(parent_1, parent_2);
            child_1 = children[0];
            child_2 = children[1];
            //child_1 = Mutate(child_1);
            //child_2 = Mutate(child_2);
            if (!population.ContainsKey(child_1))
            {
                population.Add(child_1, 100);
            }
            if (!population.ContainsKey(child_2))
            {
                population.Add(child_2, 100);
            }
            SortByErrors();
            if (population[child_1] <= rankedPopulationList[0].Value)
            {
                Console.WriteLine(child_1 + " - " + population[child_1]);
            }
            if (population[child_2] <= rankedPopulationList[0].Value)
            {
                Console.WriteLine(child_2 + " - " + population[child_2]);
            }
            Console.WriteLine("population: " + population.Count);

            setColors(rankedPopulationList[0].Key);

            if (rankedPopulationList[0].Value == 0) {
                setColors(rankedPopulationList[0].Key);
                dispatcherTimer.Stop();
                sw.Stop();
                Console.Write(gettime());
            }
            text.Text = rankedPopulationList[0].Key + " errors " + population[rankedPopulationList[0].Key]  + " Time Passed:" + gettime().Substring(0,5) + " Sec." ;
        }

        private string gettime()
        {
            return sw.Elapsed.TotalSeconds + " Sec.";
        }

        private string Mutate(string child_1)
        {
            string chromosomeToBeMutated = child_1;
            string mutatedChromosome = RandomMutate(chromosomeToBeMutated);
            return mutatedChromosome;
        }

        private string getFittedParent()
        {
            int errorMask = 1;
            int count = 0;
            int parent_error = 100;
            string fitted_parent ="";
            while ((parent_error > rankedPopulationList[0].Value + errorMask ) || (fitted_parent.Equals(parent_1)))
            {
                count++;
                fitted_parent = rankedPopulationList[rnd.Next(rankedPopulationList.Count)].Key;
                parent_error = population[fitted_parent];
                if (count > 100) {
                    errorMask++;
                    count = 0;
                }

            }

            return fitted_parent;
        }

        private void SortByErrors()
        {
            Dictionary<string, int> populationDictionary = new Dictionary<string, int>();
            for (int i = 0; i < population.Count; i++)
            {
                
                string currentChromosome = population.ToList()[i].Key;
                int error = getErrorFrom(currentChromosome);
                population[currentChromosome] = error;
                //rankedPopulation[i].Value(error);
                //var newEntry = new KeyValuePair<string,int>(rankedPopulation.);
                //errorlist.Count;
            }
            rankedPopulationList = population.ToList();
            rankedPopulationList.Sort(
                delegate(KeyValuePair<string, int> pair1, KeyValuePair<string, int> pair2) {
                    return pair1.Value.CompareTo(pair2.Value); 
                }
            );
        }

        private int getErrorFrom(string chromosome)
        {
            ArrayList errorlist = new ArrayList();
            for (int i = 0; i < chromosome.Length; i++)
			{
                GraphMap n = (GraphMap)graphs[i];
                foreach (int child in n.chlidren) {
                    GraphMap cur = (GraphMap)graphs[child - 1];
                    if (chromosome.Substring(i,1).Equals(chromosome.Substring(child - 1,1))) {
                        if (errorlist.Count == 0)
                        {
                            errorlist.Add(new int[] { n.ID, cur.ID });
                        }
                        else
                        {
                            bool exists = false;
                            foreach (int[] entry in errorlist)
                            {
                                if ((entry[0].Equals(n.ID) && entry[1].Equals(cur.ID)) || (entry[0].Equals(cur.ID) && entry[1].Equals(n.ID)))
                                {
                                    exists = true;
                                }
                            }
                            if (!exists)
                            {
                                errorlist.Add(new int[] { n.ID, cur.ID });
                            }
                        }
                    }
                }
            }
            return errorlist.Count;
        }

        private void populate()
        {
            for (int i = 0; i < population_start; i++)
            {
                string generated_cromosome = "";
                for (int y = 0; y < graphs.Count; y++)
                {
                    int rand = rnd.Next(colorsToUse);
                    generated_cromosome += rand + "";
                }
                population.Add(generated_cromosome, 100);
            }
            SortByErrors();
        }

        private void setColors(string chromosome)
        {
            for (int i = 0; i < chromosome.Length; i++)
            {
                GraphMap gm = (GraphMap)graphs[i];
                Rectangle b = (Rectangle)graph1.Children[i];
                switch (chromosome.Substring(i, 1))
                {
                    case "0":
                        b.Fill = Brushes.Red;
                        break;
                    case "1":
                        b.Fill = Brushes.Blue;
                        break;
                    case "2":
                        b.Fill = Brushes.Green;
                        break;
                    case "3":
                        b.Fill = Brushes.Yellow;
                        break;
                };
            }
        }
     
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (rankedPopulationList != null) {
                rankedPopulationList.Clear();
            }
            population.Clear();
            population_start = 50;
            sw = System.Diagnostics.Stopwatch.StartNew();
            sw.Start();
            populate();
            setColors(rankedPopulationList[0].Key);
            
            Begin();
        }

        private void Begin()
        {
            dispatcherTimer.Start();
        }

        //step click
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            step();
        }

        private void step()
        {
            dispatcherTimer.Stop();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            step();
        }
        
    }
}

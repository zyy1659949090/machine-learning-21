﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithms
{
    class Population
    {
        private readonly int _size;
        private readonly double _mutationRate;
        private readonly int _lengthOfAnswer;
        private static string _target;
        public IList<Genome> Genomes { get; set; }
		public IList<Genome> MatingPool { get; set; }

        public Population(int size, double mutationRate, string target)
        {
            _size = size;
            _mutationRate = mutationRate;
            _lengthOfAnswer = target.Length;
            _target = target;
            Genomes = InitialisePopulation().ToList();
        }

        public void GetNextGeneration()
        {
			Genomes = Rank().ToList();
            var random = new Random(DateTime.Now.Millisecond);
            for (int i = (_size/5)+1; i < _size; i++)
            {
				int random1 = random.Next(MatingPool.Count);
				int random2 = random.Next(MatingPool.Count);
				Genomes[i] = FitnessEvaluator.CrossoverGenomes(MatingPool[random1], MatingPool[random2]);
                Genomes[i] = FitnessEvaluator.MutateGenome(Genomes[i], _mutationRate);
            }
        }

        public IOrderedEnumerable<Genome> Rank()
        {
			// get fitness and normalise
			MatingPool = new List<Genome>();
			foreach (Genome genome in Genomes) {
				genome.Fitness = FitnessEvaluator.GetFitness (genome, _target);
			}
			var totalFitness = Genomes.Sum(g => g.Fitness);
			foreach (Genome genome in Genomes) {
				genome.NormalisedFitness = (genome.Fitness / totalFitness) * 100;
				for (int i = 0; i < genome.NormalisedFitness + 1; i++) {
					MatingPool.Add (genome);
				}
			}

			// order the genomes by fitness
			return Genomes.OrderByDescending(g => g.Fitness);
        }

        private IEnumerable<Genome> InitialisePopulation()
        {
            for (int i = 0; i < _size; i++)
            {
                yield return new Genome(_lengthOfAnswer);
            }
        }
    }


}

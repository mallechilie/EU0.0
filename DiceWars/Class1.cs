using System;
using System.Linq;
using MapBuilder;
using Superbest_random;

namespace DiceWars
{
	public class DiceWars
	{
		public static Random R;
		public static int ForceLimit;

	}

	public static class AiMath
	{
		/// <summary>
		/// Calculates the chance to throw higher than the player2. 
		/// </summary>
		/// <remarks> Chances are calculated by assuming normal distributions, so small number of dice may give unexpected results. </remarks>
		/// <param name="dice1"></param>
		/// <param name="dice2"></param>
		/// <returns></returns>
		public static float ThrowsHigherThan(int dice1, int dice2)
		{
			return HigherNormalDstr(3.5 * dice1, 70d / 24d * dice1, 3.5 * dice2, 70d / 24d * dice2);
		}
		public static float HigherNormalDstr(double mu1, double sigma1, double mu2, double sigma2)
		{
			double mu = mu1 - mu2;
			double sigma = Math.Sqrt(sigma1 * sigma1 + sigma2 * sigma2);
			return (float)(1 - Phi(-mu / sigma));
		}
		public static double Phi(double x)
		{
			// constants
			double a1 = 0.254829592;
			double a2 = -0.284496736;
			double a3 = 1.421413741;
			double a4 = -1.453152027;
			double a5 = 1.061405429;
			double p = 0.3275911;

			// Save the sign of x
			int sign = 1;
			if (x < 0)
				sign = -1;
			x = Math.Abs(x) / Math.Sqrt(2.0);

			// A&S formula 7.1.26
			double t = 1.0 / (1.0 + p * x);
			double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

			return 0.5 * (1.0 + sign * y);
		}

	}

	public class DwProvinceInfo : ProvinceInfo
	{
		public int Dice;

		public DwProvinceInfo(Province province) : base(province)
		{
			Dice = 1;
		}

		public void Invade(DwProvinceInfo neighbour, int dice)
		{
			if (dice >= Dice)
				throw new ArgumentOutOfRangeException("Always Leave 1 die behind.");
			int ourRoll = DiceWars.R.NextDice(dice);
			int theirRoll = DiceWars.R.NextDice(neighbour.Dice);
			while(ourRoll==theirRoll)
			{
				ourRoll = DiceWars.R.NextDice(dice);
				theirRoll = DiceWars.R.NextDice(neighbour.Dice);
			}
			Dice -= dice;
			if(ourRoll>theirRoll)
			{
				neighbour.Dice = dice;
				Province.Nation.Add(neighbour.Province);
			}
		}
	}

	public class DwNationInfo : NationInfo
	{
		public int DiceReserve = 0;
		public int DicePerTurn { get => Nation.Provinces.Count; }

		public DwNationInfo(Nation nation) : base(nation)
		{
		}

		public void Reinforce()
		{
			DiceReserve += DicePerTurn;
			// TODO: fix the typecast, there has to be a better way.
			for (var unfilledProvinces = Nation.Provinces.Where(p => ((DwProvinceInfo)p.ProvinceInfo).Dice < DiceWars.ForceLimit);
				unfilledProvinces.Count() > 0 && DiceReserve > 0;
				unfilledProvinces = Nation.Provinces.Where(p => ((DwProvinceInfo)p.ProvinceInfo).Dice < DiceWars.ForceLimit)) 
			{
				Province p = unfilledProvinces.ElementAt(DiceWars.R.Next(unfilledProvinces.Count()));
				((DwProvinceInfo)p.ProvinceInfo).Dice++;
				DiceReserve--;
			}
		}
	}
}

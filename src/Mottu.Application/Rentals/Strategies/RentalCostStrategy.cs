using Mottu.Application.Common.Interfaces;
using Mottu.Application.Rentals.Commands;
using Mottu.Domain.Entities;
using System;

namespace Mottu.Application.Rentals.Strategies
{
    public class EarlyReturnCostStrategy : IRentalCostStrategy
    {
        public ReturnRentalResult CalculateCosts(Rental rental, DateTime returnDate)
        {
            var totalDays = (rental.PredictedEndDate.Date - rental.StartDate.Date).Days;
            var usedDays = (returnDate.Date - rental.StartDate.Date).Days;
            // usedDays += 1; 
            var unusedDays = totalDays - usedDays;
            var dailyRate = rental.TotalCost / (rental.PredictedEndDate - rental.StartDate).Days;

            var penaltyRate = rental.Plan switch
            {
                RentalPlan.SevenDays => 0.20m,
                RentalPlan.FifteenDays => 0.40m,
                RentalPlan.ThirtyDays => 0.60m,
                _ => throw new InvalidOperationException("Invalid rental plan."),
            };

            var penaltyCharges = unusedDays * dailyRate * penaltyRate;
            var totalCost = usedDays * dailyRate + penaltyCharges;

            return new ReturnRentalResult
            {
                TotalCost = totalCost,
                AdditionalCharges = 0m,
                PenaltyCharges = penaltyCharges
            };
        }
    }

    public class LateReturnCostStrategy : IRentalCostStrategy
    {
        public ReturnRentalResult CalculateCosts(Rental rental, DateTime returnDate)
        {
            var extraDays = (returnDate.Date - rental.PredictedEndDate.Date).Days;
            var additionalCharges = extraDays * 50m;
            var totalCost = rental.TotalCost + additionalCharges;

            return new ReturnRentalResult
            {
                TotalCost = totalCost,
                AdditionalCharges = additionalCharges,
                PenaltyCharges = 0m
            };
        }
    }

    public class OnTimeReturnCostStrategy : IRentalCostStrategy
    {
        public ReturnRentalResult CalculateCosts(Rental rental, DateTime returnDate)
        {
            return new ReturnRentalResult
            {
                TotalCost = rental.TotalCost,
                AdditionalCharges = 0m,
                PenaltyCharges = 0m
            };
        }
    }
}

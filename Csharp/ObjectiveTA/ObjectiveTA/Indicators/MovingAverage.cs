﻿using System;
using ObjectiveTA.Objects.Input;
using ObjectiveTA.Objects.Output;

namespace ObjectiveTA.Indicators
{
    public static class MovingAverage
    {
        /// <summary>
        /// Simple Moving Average
        /// </summary>
        /// <returns>The SMA.</returns>
        /// <param name="candleSticks">Candle stick data</param>
        /// <param name="period">Time period (size of candlestick time inerval * period)</param>
        /// <param name="priceSource">Price Source (enum: open, high, low, close)</param>
        public static MAModel SMA(CandleStickCollection candleSticks, int period = 14, 
                                  PriceSource priceSource = PriceSource.Close)
        {
            int count = candleSticks.Count;
            double[] priceArray = priceSource.GetArrayFromCandleStickCollection(candleSticks);
            double[] sma = new double[count];
            double sum = priceArray[0];

            // Compute the first sum
            for (int i = 1; i < period - 1; i++)
            {
                sum = sum + priceArray[i];
            }

            sma[period-1] = sum / period;

            for (int j = period; j < count - 1; j++)
            {
                // No more iterating required for the other sums
                sum = sum - priceArray[j - period] + priceArray[j];
                sma[j] = sum/period;
            }
            
            return new MAModel(sma, MAType.SMA);
        }

        /// <summary>
        /// Exponential Moving Average
        /// </summary>
        /// <returns>The EMA.</returns>
        /// <param name="candleSticks">Candle sticks.</param>
        /// <param name="period">Period.</param>
        /// <param name="priceSource">Price source.</param>
        public static MAModel EMA(CandleStickCollection candleSticks, int period = 14,
                                  PriceSource priceSource = PriceSource.Close)
        {
            int count = candleSticks.Count;
            double[] ema = new double[count];

            double w = 1 / period;

            // Set intial value to first price value
            ema[0] = priceSource.GetValueFromCandleStick(candleSticks[0]);

            for (int i = 1; i < count; i++)
            {
                ema[i] = w * priceSource.GetValueFromCandleStick(candleSticks[i])
                                        + (1.0 - w)*ema[i-1];
            }

            return new MAModel(ema, MAType.EMA);
        }


        /// <summary>
        /// Cumulative Moving Average
        /// </summary>
        /// <returns>The cma.</returns>
        /// <param name="candleSticks">Candle sticks.</param>
        /// <param name="priceSource">Price source.</param>
        public static MAModel CMA(CandleStickCollection candleSticks,
                                  PriceSource priceSource = PriceSource.Close)
        {
            int count = candleSticks.Count;
            double[] cma = new double[count];
            double[] priceArray = priceSource.GetArrayFromCandleStickCollection(candleSticks);
            cma[0] = priceArray[0];

            for (int i = 1; i < count; i++)
            {
                cma[i] = cma[i - 1] + (priceArray[i] - cma[i - 1]) / (double)i;
            }

            return new MAModel(cma, MAType.CMA);
        }

        /// <summary>
        /// Weighted Moving Average
        /// </summary>
        /// <returns>The wma.</returns>
        /// <param name="candleSticks">Candle sticks.</param>
        /// <param name="weight">Weight.</param>
        /// <param name="priceSource">Price source.</param>
        public static MAModel WMA(CandleStickCollection candleSticks, int weight =14,
                                  PriceSource priceSource = PriceSource.Close)
        {
            int count = candleSticks.Count;
            double[] priceArray = priceSource.GetArrayFromCandleStickCollection(candleSticks);
            double[] wma = new double[count];
            double[] weights = new double[weight];
            double sum = weight * (weight + 1) / 2;

            for (int i = 0; i < weight; i++)
            {
                weights[i] = i / sum;
            }

            for (int i = weight-1; i < count; i++)
            {
                for (int j = 0; j < weight; j++)
                {
                    wma[i] = wma[i] + priceArray[j + i] * weight;
                }
            }

            return new MAModel(wma, MAType.WMA);
        }

        /// <summary>
        /// Smoothed Moving Average/Running Moving Average
        /// </summary>
        /// <returns>The smma.</returns>
        /// <param name="candleSticks">Candle sticks.</param>
        /// <param name="period">Period.</param>
        /// <param name="priceSource">Price source.</param>
        public static MAModel SMMA(CandleStickCollection candleSticks, int period = 14,
                                   PriceSource priceSource = PriceSource.Close)
        {
            int count = candleSticks.Count;
            double[] smma = new double[count];
            double[] priceArray = priceSource.GetArrayFromCandleStickCollection(candleSticks);

            double sum = priceArray[0];

            //Iterate for first sum over period n
            for (int i = 0; i < period - 1; i++)
            {
                sum = sum + priceArray[i];
            }

            // First n values are zero
            smma[period-1] = sum / period;

            for (int i = period; i < count; i++)
            {
                // No need to iterate through every sum
                sum = sum - priceArray[i - period] + priceArray[i];

                smma[i] = (sum - smma[0] + priceArray[i]) / (double)period;
            }

            return new MAModel(smma, MAType.SMMA);
        }
    }



}

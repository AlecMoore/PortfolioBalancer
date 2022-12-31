﻿using BitfinexApi;

namespace BotCore
{
    public class program
    {
        private static Fng btcFng;
        private static BalancesResponse balances;
        private static int price;
        private static decimal cycleAmount;
        private static int buyThreshold = 20;
        private static int sellThreshold = 80;
        private static OrderSide cycleType;
        private static BitfinexApiV1 api;


        private static void setUp()
        {
            api = new BitfinexApiV1("cVEc61LYLYUtXFxpSf668FuST5m5WfXRkx2M7xgbq4H", "bBqKbdGFT8YVRSvTNBVMPaGaPaBJuq5ZEfbacU97BSH");
        }

        private static async Task fngBotAsync()
        {

            await GetDataAsync();
            await Logic();

        }

        private static double ConvertUSDToBTC(double usdAmount)
        {
            return usdAmount / price;
        }

        private static async Task Logic()
        {

            if (btcFng.value <= buyThreshold)
            {
                cycleType = OrderSide.Buy;
                cycleAmount = (decimal)ConvertUSDToBTC((double)balances.totalAvailableUSD / 4.0); //need to convert to BTC amount
                                                                                                  //await Cycle();
            }
            else if (btcFng.value >= sellThreshold)
            {
                cycleType = OrderSide.Sell;
                cycleAmount = balances.totalAvailableUSD / (decimal)10.0;
            }
            else
            {
                return;
            }

            var call = api.ExecuteOrder(OrderSymbol.BTCUSD, cycleAmount, price, OrderExchange.Bitfinex, cycleType, OrderType.MarginMarket);
        }

        private static async Task GetDataAsync()
        {
            //Console.WriteLine("GetData");
            btcFng = await FngApiUtil.ReadFNG();
            balances = api.GetBalances();
            price = await CoinGeckoUtil.readPrice();
        }

        public static async Task Main(string[] args)
        {
            setUp();

            //await fngBotAsync();

            //Console.WriteLine("wank");

            ActiveOrdersResponse wank = api.GetActiveOrders();

        }
    }
}

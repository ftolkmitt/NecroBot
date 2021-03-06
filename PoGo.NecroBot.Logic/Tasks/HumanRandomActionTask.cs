﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PoGo.NecroBot.Logic.State;

namespace PoGo.NecroBot.Logic.Tasks
{
    public class HumanRandomActionTask
    {
        private static Random ActionRandom = new Random();

        public static async Task Execute(ISession session, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var randomCommand = Enumerable.Range(1, 8).OrderBy(x => ActionRandom.Next()).Take(8).ToList();
            for (int i = 0; i < 8; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                TinyIoC.TinyIoCContainer.Current.Resolve<MultiAccountManager>().ThrowIfSwitchAccountRequested();
                switch (randomCommand[i])
                {
                    case 1:
                        // Handling of Eggs and Incubators preferably needs to happen every time.
                        // While monitoring the previous setting of 50% propability we ended up with 
                        // over an hour of execution without setting any new Eggs for incubation (entering
                        // UseEggIncubators).
                        if (session.LogicSettings.UseEggIncubators)
                            if (ActionRandom.Next(1, 10) > 0)
                                await UseIncubatorsTask.Execute(session, cancellationToken);
                        break;
                    case 2:
                        if (session.LogicSettings.TransferDuplicatePokemon)
                            if (ActionRandom.Next(1, 10) > 4)
                                await TransferDuplicatePokemonTask.Execute(session, cancellationToken);
                        if (session.LogicSettings.TransferWeakPokemon)
                            if (ActionRandom.Next(1, 10) > 4)
                                await TransferWeakPokemonTask.Execute(session, cancellationToken);
                        if (ActionRandom.Next(1, 10) > 4)
                        {
                            if (session.LogicSettings.EvolveAllPokemonAboveIv ||
                                session.LogicSettings.EvolveAllPokemonWithEnoughCandy ||
                                session.LogicSettings.UseLuckyEggsWhileEvolving ||
                                session.LogicSettings.KeepPokemonsThatCanEvolve)
                            {
                                await EvolvePokemonTask.Execute(session, cancellationToken);
                            }
                        }
                        break;
                    case 3:
                        if (session.LogicSettings.UseLuckyEggConstantly)
                            if (ActionRandom.Next(1, 10) > 4)
                                await UseLuckyEggConstantlyTask.Execute(session, cancellationToken);
                        break;
                    case 4:
                        if (session.LogicSettings.UseIncenseConstantly)
                            if (ActionRandom.Next(1, 10) > 4)
                                await UseIncenseConstantlyTask.Execute(session, cancellationToken);
                        break;
                    case 5:
                        if (session.LogicSettings.RenamePokemon)
                            if (ActionRandom.Next(1, 10) > 4)
                                await RenamePokemonTask.Execute(session, cancellationToken);
                        break;
                    case 6:
                        if (session.LogicSettings.AutoFavoritePokemon)
                            if (ActionRandom.Next(1, 10) > 4)
                                await FavoritePokemonTask.Execute(session, cancellationToken);
                        break;
                    case 7:
                        if (ActionRandom.Next(1, 10) > 4)
                            await RecycleItemsTask.Execute(session, cancellationToken);
                        break;
                    case 8:
                        if (session.LogicSettings.AutomaticallyLevelUpPokemon)
                            if (ActionRandom.Next(1, 10) > 4)
                                await LevelUpPokemonTask.Execute(session, cancellationToken);
                        break;
                }
            }

            GetPokeDexCount.Execute(session, cancellationToken);
        }

        public static async Task TransferRandom(ISession session, CancellationToken cancellationToken)
        {
            if (ActionRandom.Next(1, 10) > 4)
                await TransferDuplicatePokemonTask.Execute(session, cancellationToken);
        }
    }
}
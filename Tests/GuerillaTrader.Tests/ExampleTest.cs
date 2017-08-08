//using Abp.Domain.Repositories;
//using Abp.Domain.Uow;
//using MachineLearningBP.Entities.Sports.Nba;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;
//using Shouldly;
//using MachineLearningBP.Services.Sports.Nba;
//using MachineLearningBP.Entities.Sports.Nfl;
//using MachineLearningBP.Core.Services.Sports.Nfl;
//using MachineLearningBP.Services.Sports.Nfl;
//using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;

//namespace MachineLearningBP.Tests.Sports.Nba
//{
//    public class Nfl_Tests : MachineLearningBPTestBase
//    {
//        private readonly IRepository<NflTeam> _nflTeamRepository;
//        private readonly IUnitOfWorkManager _unitOfWorkManager;
//        readonly INflGameDomainService _nflGameDomainService;
//        readonly INflPointsExampleDomainService _nflPointsExampleDomainService;

//        public Nfl_Tests()
//        {
//            _nflTeamRepository = Resolve<IRepository<NflTeam>>();
//            _unitOfWorkManager = Resolve<IUnitOfWorkManager>();
//            _nflGameDomainService = Resolve<INflGameDomainService>();
//            _nflPointsExampleDomainService = Resolve<INflPointsExampleDomainService>();
//        }

//        //[Fact]
//        public async Task NflTeamCount_Test()
//        {
//            using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin())
//            {
//                int teamCount = await _nflTeamRepository.CountAsync();
//                teamCount.ShouldBe(30);

//                uow.Complete();
//            }
//        }

//        [Fact(DisplayName = "NflPoints.FindOptimalParametersPython")]
//        public async Task NflPointsFindOptimalParametersPython_Test()
//        {
//            await _nflPointsExampleDomainService.FindOptimalParametersPython(true);
//        }

//        [Fact(DisplayName = "NflPoints.NflPointsGeneticOptimize")]
//        public async Task NflPointsGeneticOptimize_Test()
//        {
//            GeneticOptimizeInput input = new GeneticOptimizeInput();
//            input.GuessMethod = KNearestNeighborsGuessMethods.WeightedKnn;
//            input.WeightMethod = KNearestNeighborsWeightMethods.InverseWeight;
//            input.Trials = 25;
//            input.K = 15;
//            input.popsize = 50;
//            input.step = 1;
//            input.mutprob = .20;
//            input.elite = .20;
//            input.maxiter = 100;

//            await _nflPointsExampleDomainService.GeneticOptimize(input);
//        }

//        [Fact(DisplayName = "NflGames.PopulateSamples")]
//        public async Task NflGamesPopulateSamples()
//        {
//            await _nflGameDomainService.PopulateSamples();
//        }
//    }
//}

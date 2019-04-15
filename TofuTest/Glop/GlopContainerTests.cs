using NSubstitute;
using NUnit.Framework;
using TofuCore.Events;
using TofuCore.Glops;
using TofuCore.Service;

namespace TofuTest
{
    public class GlopContainerTests
    {
        private GlopContainer<Glop> _glopContainer;

        private IServiceContext _subServiceContext;
        private IEventContext _subEventContext;

        [SetUp]
        public void SetUp()
        {
            //Prepare Bindings
            _subServiceContext = Substitute.For<IServiceContext>();
            _subEventContext = Substitute.For<IEventContext>();
            _subServiceContext.Has("IEventContext").Returns(true);
            _subServiceContext.Fetch("IEventContext").Returns(_subEventContext);

            //Prepare Events
            TofuEvent _frameUpdateEvent = new TofuEvent("FrameUpdate");
            _subEventContext.GetEvent("FrameUpdate").Returns(_frameUpdateEvent);

            //Prepare Object Under Test
            _glopContainer = new GlopContainer<Glop>();
            _glopContainer.BindServiceContext(_subServiceContext);
            _glopContainer.ResolveServiceBindings();
            _glopContainer.Build();
            _glopContainer.Initialize();
        }

        [Test]
        public void TestGlopContainerDependenciesSatisfied()
        {
            Assert.True(_glopContainer.CheckDependencies());
        }

        [Test]
        public void TestGlopContainerShouldInitialize()
        {
            Assert.AreEqual(0, _glopContainer.GetContents().Count);
        }

        [Test]
        public void TestGlopContainerShouldGetIdFromServiceContext()
        {
            _subServiceContext.LastGlopId.Returns(144);

            Assert.AreEqual(144, _glopContainer.GenerateGlopId());
            Assert.AreEqual(145, _glopContainer.GenerateGlopId());
            Assert.AreEqual(146, _glopContainer.GenerateGlopId());

            _subServiceContext.LastGlopId.Returns(120);
            Assert.AreEqual(120, _glopContainer.GenerateGlopId());
        }

        [Test]
        public void TestGlopContainerShouldRegisterGlop()
        {
            Glop glop1 = Substitute.For<Glop>();
            Glop glop2 = Substitute.For<Glop>();
            _subServiceContext.LastGlopId.Returns(144);

            _glopContainer.Register(glop1);
            _glopContainer.Register(glop2);

            Assert.AreEqual(144, glop1.Id);
            Assert.AreEqual(145, glop2.Id);
            Assert.AreEqual(2, _glopContainer.CountActive());
            Assert.True(_glopContainer.HasId(144));
            Assert.True(_glopContainer.HasId(145));

        }

        [Test]
        public void TestGlopContainerShouldRemoveGlop()
        {
            Glop glop1 = Substitute.For<Glop>();
            _subServiceContext.LastGlopId.Returns(144);
            _glopContainer.Register(glop1);

            Assert.AreEqual(1, _glopContainer.CountActive());
            Assert.True(_glopContainer.HasId(144));

            _glopContainer.Destroy(glop1);

            Assert.AreEqual(0, _glopContainer.CountActive());
            Assert.False(_glopContainer.HasId(144));
        }

        [Test]
        public void TestGlopContainerShouldIgnoreNullRegister()
        {
            _glopContainer.Register(null);

            Assert.AreEqual(0, _glopContainer.CountActive());
        }

        [Test]
        public void TestGlopContainerShouldIgnoreDestroyOfGlopRegisteredElsewhere()
        {
            GlopContainer<Glop> _glopContainer2 = new GlopContainer<Glop>();
            _glopContainer2.BindServiceContext(_subServiceContext);
            _glopContainer2.ResolveServiceBindings();
            _glopContainer2.Build();
            _glopContainer2.Initialize();

            Glop glop1 = Substitute.For<Glop>();
            _glopContainer2.Register(glop1);
            Assert.AreEqual(1, _glopContainer2.CountActive());

            _glopContainer.Destroy(glop1);
            Assert.AreEqual(1, _glopContainer2.CountActive());
        }
    }
}


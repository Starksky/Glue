using System;
using _Project._Common.Scripts.Contracts.Interfaces;
using _Project.Features.Player.Scripts.Contracts;
using _Project.Features.Player.Scripts.Presentation;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;

namespace _Project.Features.Player.Tests
{
    [TestFixture]
    public class PlayerSlingshotPresenterTests
    {
        IPhysicBody2D _physicBody;
        IPlayerSlingshotService _service;
        IPlayerSlingshotConfig _config;
        PlayerSlingshotPresenter _presenter;
        
        [SetUp]
        public void Setup()
        {
            _physicBody = Substitute.For<IPhysicBody2D>();
            _service = Substitute.For<IPlayerSlingshotService>();
            _config = Substitute.For<IPlayerSlingshotConfig>();
        
            _config.MaxDragDistance.Returns(1.8f);
            _config.MaxDragDistanceBody.Returns(0.25f);
            _config.AngleForForce.Returns(90f);
            _config.DragForce.Returns(15f);
            _config.ThrowForce.Returns(12f);
        
            _service.GetState().Returns(_config);

            _presenter = new PlayerSlingshotPresenter(_physicBody, _service);
            _presenter.Initialize();
        }
        
        [Test]
        public void BeginDrag_ResetsAllParameters()
        {
            _presenter.BeginDrag();
        
            Assert.AreEqual(Vector2.zero, _presenter.DeltaDrag);
            Assert.AreEqual(0f, _presenter.StrengthDrag);
        }
        
        [Test]
        public void StayDrag_WithoutBeginDrag_DoesNothing()
        {
            _presenter.StayDrag(Vector2.one, Vector2.zero);
        
            Assert.AreEqual(Vector2.zero, _presenter.DeltaDrag);
            Assert.AreEqual(0f, _presenter.StrengthDrag);
        }
        
        // ─── StayDrag (distance < MaxDragDistance) ───
        
        [Test]
        public void StayDrag_WhenDistanceLessThanMax_CalculatesCorrectly()
        {
            _physicBody.Position.Returns(Vector2.zero);
            _presenter.BeginDrag();
        
            // Мышь на расстоянии 1 (меньше MaxDragDistance = 1.8f)
            _presenter.StayDrag(new Vector2(1, 0), Vector2.zero);
        
            Assert.AreEqual(new Vector2(-1, 0), _presenter.DeltaDrag);
            Assert.AreEqual(1f / 1.8f, _presenter.StrengthDrag);  // distance / MaxDragDistance
        }
        
        // ─── StayDrag (distance > MaxDragDistance) ───
        
        [Test]
        public void StayDrag_WhenDistanceExceedsMax_ClampsDelta()
        {
            _physicBody.Position.Returns(Vector2.zero);
            _presenter.BeginDrag();
        
            // Мышь на расстоянии 2 (больше MaxDragDistance = 1.8f)
            _presenter.StayDrag(new Vector2(2, 0), Vector2.zero);
        
            // Delta обрезан до MaxDragDistance
            Assert.AreEqual(1.8f, _presenter.DeltaDrag.magnitude, 0.01f);
            Assert.AreEqual(1f, _presenter.StrengthDrag);  // 5/5 = 1
        }
        
        // ─── EndDrag (strength > 0.1f) ───

        [Test]
        public void EndDrag_WhenStrengthSufficient_AppliesForce()
        {
            _physicBody.Position.Returns(Vector2.zero);
            _presenter.BeginDrag();
            _presenter.StayDrag(new Vector2(3f, 0), Vector2.zero);
            _presenter.EndDrag(Vector2.zero);
        
            // Проверяем, что импульс был применён
            _physicBody.Received(1).AddImpulse(Arg.Any<Vector2>());
            _physicBody.Received(1).AddTorqueImpulse(Arg.Any<float>());
        }

        // ─── EndDrag (strength <= 0.1f) ───

        [Test]
        public void EndDrag_WhenStrengthTooLow_DoesNotApplyForce()
        {
            _physicBody.Position.Returns(Vector2.zero);
            _presenter.BeginDrag();
            // Мышь очень близко — strength = 0.083f
            _presenter.StayDrag(new Vector2(0.15f, 0), Vector2.zero);
            _presenter.EndDrag(Vector2.zero);
        
            _physicBody.DidNotReceive().AddImpulse(Arg.Any<Vector2>());
        }

        // ─── EndDrag сбрасывает состояние ───

        [Test]
        public void EndDrag_ResetsAllParameters()
        {
            _physicBody.Position.Returns(Vector2.zero);
            _presenter.BeginDrag();
            _presenter.StayDrag(new Vector2(3, 0), Vector2.zero);
            _presenter.EndDrag(Vector2.zero);
        
            Assert.AreEqual(Vector2.zero, _presenter.DeltaDrag);
            Assert.AreEqual(0f, _presenter.StrengthDrag);
        }

        // ─── OnChangeState ───

        [Test]
        public void OnChangeState_UpdatesConfig()
        {
            var newConfig = Substitute.For<IPlayerSlingshotConfig>();
            newConfig.MaxDragDistance.Returns(5f);
        
            // Симулируем вызов события
            _service.OnPlayerSlingshotStateChange += 
                Raise.Event<Action<IPlayerSlingshotConfig>>(newConfig);
        
            // Проверяем через StayDrag с новыми параметрами
            _physicBody.Position.Returns(Vector2.zero);
            _presenter.BeginDrag();
            _presenter.StayDrag(new Vector2(4f, 0), Vector2.zero);
        
            // Delta должен быть обрезан до нового MaxDragDistance = 5
            Assert.AreEqual(4f, _presenter.DeltaDrag.magnitude, 0.01f);
        }
    }
}
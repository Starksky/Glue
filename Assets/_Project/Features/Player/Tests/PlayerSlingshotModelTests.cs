using _Project.Features.Player.Scripts.Contracts.DTOs;
using _Project.Features.Player.Scripts.Domain;
using NUnit.Framework;

namespace _Project.Features.Player.Tests
{
    [TestFixture]
    public class PlayerSlingshotModelTests
    {
        [Test]
        public void AddThrowForcePercent_IncreasesThrowForce()
        {
            // Arrange
            var config = new PlayerSlingshotState(
                0f, 
                0f, 
                5f, 
                0f, 
                0f);
            var model = new PlayerSlingshotModel(config);

            // Act
            model.AddThrowForcePercent(30);

            // Assert
            Assert.AreEqual(6.5f, model.ThrowForce);
        }
        
        [Test]
        public void AddThrowForcePercent_DecreasesThrowForce()
        {
            // Arrange
            var config = new PlayerSlingshotState(
                0f, 
                0f, 
                5f, 
                0f, 
                0f);
            var model = new PlayerSlingshotModel(config);

            // Act
            model.AddThrowForcePercent(-35);

            // Assert
            Assert.AreEqual(3.25f, model.ThrowForce);
        }
        
        [Test]
        public void AddThrowForcePercent_Negative_ClampsToZero()
        {
            // Arrange
            var config = new PlayerSlingshotState(
                0f, 
                0f, 
                5f, 
                0f, 
                0f);
            var model = new PlayerSlingshotModel(config);

            // Act
            model.AddThrowForcePercent(-200);  // -200% → пытается уйти в -100%

            // Assert
            Assert.AreEqual(5f, model.ThrowForce);  // не падает ниже 0
        }
        
        [Test]
        public void GetState_ReturnsCorrectValues()
        {
            // Arrange
            var config = new PlayerSlingshotState(
                2f, 
                1f, 
                5f, 
                45f, 
                10f);
            
            var model = new PlayerSlingshotModel(config);
            
            // Act
            model.AddThrowForcePercent(30);
            var state = model.GetState();

            // Assert
            Assert.AreEqual(6.5f, state.ThrowForce);
            Assert.AreEqual(2f, state.MaxDragDistanceBody);
            Assert.AreEqual(1f, state.MaxDragDistance);
            Assert.AreEqual(45f, state.AngleForForce);
            Assert.AreEqual(10f, state.DragForce);
        }
    }
}
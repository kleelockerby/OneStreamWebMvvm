using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using OneStreamWebMvvm;
using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm.Tests
{
    public class MessageAggregatorTests
    {
        [Fact]
        public void TestLengthEqualZero()
        {
            Mock<IMessageAggregator> messageAggregatorMock = new Mock<IMessageAggregator>();

            //arrange
            MessageViewModel messageViewModel = new MessageViewModel(messageAggregatorMock.Object);

            //Act
            messageViewModel.MessageText =  "Mvvm Title";
            bool isLength = messageViewModel.CanGetMessage();

            //Assert
            Assert.Equal(isLength, messageViewModel.MessageText.Length > 0);
        }
        
    }
}

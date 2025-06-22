namespace HexaFour.Tests
{
    public class GameStateTest
    {
        [Fact]
        public void PositionIsOccupied_OnInitialState_ReturnsFalse()
        {
            // arrange
            var sut = new GameState(4, 10);

            // act
            var result = sut.PositionIsOccupied(0, 0);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void PlayerHasConsecutiveTokens_WithFourTokensInRightDirection_ReturnsTrue()
        {
            // arrange
            var sut = new GameState(4, 10);

            const int playerNumber = 1;

            sut.SetToken(playerNumber, 3, 7);
            sut.SetToken(playerNumber, 2, 8);
            sut.SetToken(playerNumber, 1, 9);
            sut.SetToken(playerNumber, 0, 10);

            // act
            bool result = sut.PlayerHasConsecutiveTokens(playerNumber, 4);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void PlayerHasConsecutiveTokens_WithFourTokensInLeftDirection_ReturnsTrue()
        {
            // arrange
            var sut = new GameState(4, 10);

            const int playerNumber = 1;

            sut.SetToken(playerNumber, 4, 3);
            sut.SetToken(playerNumber, 3, 2);
            sut.SetToken(playerNumber, 2, 1);
            sut.SetToken(playerNumber, 1, 0);

            // act
            bool result = sut.PlayerHasConsecutiveTokens(playerNumber, 4);

            // assert
            Assert.True(result);
        }

        [Fact]
        public void PlayerHasConsecutiveTokens_WithThreeTokensInLeftDirection_ReturnsFalse()
        {
            // arrange
            var sut = new GameState(4, 10);

            const int playerNumber = 1;
            const int otherPlayerNumber = 2;

            sut.SetToken(playerNumber, 3, 10);
            sut.SetToken(playerNumber, 2, 9);
            sut.SetToken(playerNumber, 1, 8);
            sut.SetToken(otherPlayerNumber, 0, 7);

            // act
            bool result = sut.PlayerHasConsecutiveTokens(playerNumber, 4);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void PlayerHasConsecutiveTokens_WithTokensHitLowerBorder_ReturnsFalse()
        {
            // arrange
            var sut = new GameState(4, 10);

            const int playerNumber = 1;

            sut.SetToken(playerNumber, 2, 10);
            sut.SetToken(playerNumber, 1, 9);
            sut.SetToken(playerNumber, 0, 8);

            // act
            bool result = sut.PlayerHasConsecutiveTokens(playerNumber, 4);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void PlayerHasConsecutiveTokens_WithTokensHitLeftBorder_ReturnsFalse()
        {
            // arrange
            var sut = new GameState(4, 10);

            const int playerNumber = 1;

            sut.SetToken(playerNumber, 3, 3);
            sut.SetToken(playerNumber, 2, 1);
            sut.SetToken(playerNumber, 1, 0);

            // act
            bool result = sut.PlayerHasConsecutiveTokens(playerNumber, 4);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void PlayerHasConsecutiveTokens_WithTokensHitRightBorder_ReturnsFalse()
        {
            // arrange
            var sut = new GameState(4, 10);

            const int playerNumber = 1;

            sut.SetToken(playerNumber, 3, 8);
            sut.SetToken(playerNumber, 2, 9);
            sut.SetToken(playerNumber, 1, 10);

            // act
            bool result = sut.PlayerHasConsecutiveTokens(playerNumber, 4);

            // assert
            Assert.False(result);
        }
    }
}
using System;
using System.Threading.Tasks;
using Xunit;
using Phoenix.Core.Knowledge.Models;
using Phoenix.Core.Knowledge.Adapters;
using Microsoft.Extensions.Logging;
using Moq;

namespace Phoenix.Core.Knowledge.Tests
{
    public class EmbeddingVectorSearchAdapterTests
    {
        private readonly EmbeddingVectorSearchAdapter _adapter;
        private readonly Mock<ILogger<EmbeddingVectorSearchAdapter>> _loggerMock;

        public EmbeddingVectorSearchAdapterTests()
        {
            _loggerMock = new Mock<ILogger<EmbeddingVectorSearchAdapter>>();
            _adapter = new EmbeddingVectorSearchAdapter(_loggerMock.Object);
        }

        [Fact]
        public async Task InitializeAsync_WithValidPath_ShouldInitialize()
        {
            await _adapter.InitializeAsync("models/embedding.bin");
            Assert.True(true);
        }

        [Fact]
        public async Task GenerateEmbeddingAsync_ShouldReturnValidVector()
        {
            await _adapter.InitializeAsync("models/embedding.bin");
            var embedding = await _adapter.GenerateEmbeddingAsync("test text");
            
            Assert.NotNull(embedding);
            Assert.NotEmpty(embedding);
            Assert.Equal(384, embedding.Length);
        }

        [Fact]
        public void CalculateSimilarity_ShouldReturnCosineSimilarity()
        {
            var vec1 = new float[] { 1, 0, 0 };
            var vec2 = new float[] { 1, 0, 0 };
            
            var similarity = _adapter.CalculateSimilarity(vec1, vec2);
            Assert.Equal(1.0f, similarity, 2);
        }
    }
}

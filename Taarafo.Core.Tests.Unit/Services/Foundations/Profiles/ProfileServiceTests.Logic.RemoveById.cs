// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Taarafo.Core.Models.Profiles;
using Xunit;

namespace Taarafo.Core.Tests.Unit.Services.Foundations.Profiles
{
    public partial class ProfileServiceTests
    {
        [Fact]
        public async Task ShouldRemoveProfileByIdAsync()
        {
            // given
            Guid randomProfileId = Guid.NewGuid();
            Guid inputProfileId = randomProfileId;
            Profile randomProfile = CreateRandomProfile();
            Profile storageProfile = randomProfile;
            Profile inputProfile = storageProfile;
            Profile deletedProfile = inputProfile;
            Profile expectedProfile = deletedProfile.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProfileByIdAsync(inputProfileId))
                    .ReturnsAsync(storageProfile);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteProfileAsync(inputProfile))
                    .ReturnsAsync(deletedProfile);

            // when
            Profile actualProfile =
                await this.profileService.RemoveProfileByIdAsync(inputProfileId);

            // then
            actualProfile.Should().BeEquivalentTo(expectedProfile);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProfileByIdAsync(inputProfileId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProfileAsync(inputProfile),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}

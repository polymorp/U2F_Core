﻿using System;

public class RandomChallengeGeneratorTests
{
    [Theory]
	public void RandomChallengeGenerator2ChallengesShouldBeDifferent()
	{
        RandomChallengeGenerator generator = new RandomChallengeGenerator();
        byte[] firstChallenge = generator.GenerateChallenge();
        byte[] secondChallenge = generator.GenerateChallenge();

        Assert.True(firstChallenge.Length, secondChallenge.Length);
	}
}
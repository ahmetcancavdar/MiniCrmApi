using MiniCrm.Domain.Common;
using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Domain.Entities;

public class OrderVerification : BaseEntity
{
	public const int MaxFailedAttempts = 5;

	public int OrderId { get; private set; }

	public Order Order { get; private set; } = null!;

	public string CodeHash { get; private set; } = string.Empty;

	public DateTime ExpiresAtUtc { get; private set; }

	public int FailedAttemptCount { get; private set; }

	public bool IsVerified { get; private set; }

	public DateTime? VerifiedAtUtc { get; private set; }

	private OrderVerification()
	{
	}

	internal OrderVerification(
		Order order,
		string codeHash,
		DateTime expiresAtUtc,
		DateTime utcNow)
	{
		Order = order;
		OrderId = order.Id;

		SetNewCode(
			codeHash,
			expiresAtUtc,
			utcNow);
	}

	internal void Renew(
		string codeHash,
		DateTime expiresAtUtc,
		DateTime utcNow)
	{
		if (IsVerified)
		{
			throw new DomainException(
				"A verified order cannot receive a new verification code.");
		}

		SetNewCode(
			codeHash,
			expiresAtUtc,
			utcNow);
	}

	internal void EnsureCanAttempt(DateTime utcNow)
	{
		if (IsVerified)
		{
			throw new DomainException(
				"Order has already been verified.");
		}

		if (utcNow >= ExpiresAtUtc)
		{
			throw new DomainException(
				"Verification code has expired.");
		}

		if (FailedAttemptCount >= MaxFailedAttempts)
		{
			throw new DomainException(
				"Maximum verification attempts have been exceeded.");
		}
	}

	internal void RegisterFailedAttempt(DateTime utcNow)
	{
		EnsureCanAttempt(utcNow);

		FailedAttemptCount++;
	}

	internal void MarkVerified(DateTime utcNow)
	{
		EnsureCanAttempt(utcNow);

		IsVerified = true;
		VerifiedAtUtc = utcNow;
	}

	private void SetNewCode(
		string codeHash,
		DateTime expiresAtUtc,
		DateTime utcNow)
	{
		if (string.IsNullOrWhiteSpace(codeHash))
		{
			throw new DomainException(
				"Verification code hash cannot be empty.");
		}

		if (expiresAtUtc <= utcNow)
		{
			throw new DomainException(
				"Verification expiration time must be in the future.");
		}

		CodeHash = codeHash;
		ExpiresAtUtc = expiresAtUtc;
		FailedAttemptCount = 0;
		IsVerified = false;
		VerifiedAtUtc = null;
	}
}
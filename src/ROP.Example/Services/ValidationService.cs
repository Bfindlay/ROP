﻿using System;
using Microsoft.Extensions.Logging;
using ROP.Example.Models;

namespace ROP.Example.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<ValidationService> _logger;

        public ValidationService(IAccountService accountService, ILogger<ValidationService> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        private Func<TransferRequest, Result<TransferRequest, string>> ValidateFromAccount =>
            request => _accountService.AccountExists(request.AccountFrom)
                ? Result<TransferRequest, string>.NewSuccess(request)
                : Result<TransferRequest, string>.NewFailure($"AccountFrom {request.AccountFrom} not recognised");

        private Func<TransferRequest, Result<TransferRequest, string>> ValidateToAccount =>
            request => _accountService.AccountExists(request.AccountTo)
                ? Result<TransferRequest, string>.NewSuccess(request)
                : Result<TransferRequest, string>.NewFailure($"AccountTo {request.AccountTo} not recognised");

        public Func<TransferRequest, Result<TransferRequest, string>> ValidateAccounts =>
            ValidateFromAccount
                .Bind(ValidateToAccount);
    }
}

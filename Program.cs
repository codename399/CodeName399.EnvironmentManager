using System.Diagnostics;

namespace CodeName399.EnvironmentManager;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}

public sealed record EnvItem(
    string Name,
    string Label,
    string Category,
    string? DefaultValue = null,
    bool Secret = false,
    bool Required = false);

public sealed class MainForm : Form
{
    readonly List<EnvItem> items = new()
    {
        new("OPENAI_API_KEY", "OpenAI API key", "OpenAI", null, Secret:true),
        new("ANGEL_PUBLIC_IP", "Angel One public IP", "Angel One / Runtime", null),
        new("ANGEL_REGISTERED_STATIC_IP", "Angel One registered static IP", "Angel One / Runtime", null),
        new("CODENAME399_AuthServiceUrl", "Auth service URL", "Service Endpoints", "http://127.0.0.1:5001/"),
        new("CODENAME399_GatewayServiceUrl", "Gateway service URL", "Service Endpoints", "http://127.0.0.1:5000/"),
        new("CODENAME399_EquityTradingServiceUrl", "Equity Trading service URL", "Service Endpoints", "http://127.0.0.1:5109/"),
        new("CODENAME399_FutureTradingServiceUrl", "Future Trading service URL", "Service Endpoints", "http://127.0.0.1:5110/"),
        new("CODENAME399_OptionsTradingServiceUrl", "Options Trading service URL", "Service Endpoints", "http://127.0.0.1:5111/"),
        new("CODENAME399_EquityTrading_InstrumentType", "Equity instrument type", "Trading Runtime", "Equity"),
        new("CODENAME399_FutureTrading_InstrumentType", "Future instrument type", "Trading Runtime", "Futures"),
        new("CODENAME399_OptionsTrading_InstrumentType", "Options instrument type", "Trading Runtime", "Options"),
        new("CODENAME399_EMAIL_SERVICE_URL", "Email service URL", "Notifications", "http://127.0.0.1:5112"),
        new("CODENAME399_EMAIL_TO", "Notification recipient email", "Notifications", null),
        new("CODENAME399_LOG_DIRECTORY", "Log directory", "Logging", null),
        new("CODENAME399_AngelOne__ApiKey", "AngelOne / ApiKey", "AngelOne", null, Secret:true),
        new("CODENAME399_AngelOne__ClientCode", "AngelOne / ClientCode", "AngelOne", ""),
        new("CODENAME399_AngelOne__Pin", "AngelOne / Pin", "AngelOne", ""),
        new("CODENAME399_AngelOne__PublicIp", "AngelOne / PublicIp", "AngelOne", ""),
        new("CODENAME399_AngelOne__RegisteredStaticIp", "AngelOne / RegisteredStaticIp", "AngelOne", ""),
        new("CODENAME399_AngelOne__RequireRegisteredStaticIpForLiveTrading", "AngelOne / RequireRegisteredStaticIpForLiveTrading", "AngelOne", "true"),
        new("CODENAME399_AngelOne__TotpSecret", "AngelOne / TotpSecret", "AngelOne", null, Secret:true),
        new("CODENAME399_MongoDatabaseSetting__DatabaseName", "MongoDatabaseSetting / DatabaseName", "MongoDatabaseSetting", "CodeName399"),
        new("CODENAME399_MongoDatabaseSetting__Environment", "MongoDatabaseSetting / Environment", "MongoDatabaseSetting", "Local"),
        new("CODENAME399_MongoDatabaseSetting__LocalConnectionString", "MongoDatabaseSetting / LocalConnectionString", "MongoDatabaseSetting", null, Secret:true),
        new("CODENAME399_MongoDatabaseSetting__OrderIntentCollection", "MongoDatabaseSetting / OrderIntentCollection", "MongoDatabaseSetting", "OrderIntent_Futures"),
        new("CODENAME399_MongoDatabaseSetting__ProjectCollection", "MongoDatabaseSetting / ProjectCollection", "MongoDatabaseSetting", "Project"),
        new("CODENAME399_MongoDatabaseSetting__RefreshTokenCollection", "MongoDatabaseSetting / RefreshTokenCollection", "MongoDatabaseSetting", "RefreshToken"),
        new("CODENAME399_MongoDatabaseSetting__RoleCollection", "MongoDatabaseSetting / RoleCollection", "MongoDatabaseSetting", "Role"),
        new("CODENAME399_MongoDatabaseSetting__StockPerformanceCollection", "MongoDatabaseSetting / StockPerformanceCollection", "MongoDatabaseSetting", "StockPerformance_Futures"),
        new("CODENAME399_MongoDatabaseSetting__TradeOrderCollection", "MongoDatabaseSetting / TradeOrderCollection", "MongoDatabaseSetting", "TradeOrder_Futures"),
        new("CODENAME399_MongoDatabaseSetting__TradingConfigurationCollection", "MongoDatabaseSetting / TradingConfigurationCollection", "MongoDatabaseSetting", "TradingConfiguration_Futures"),
        new("CODENAME399_MongoDatabaseSetting__UserCollection", "MongoDatabaseSetting / UserCollection", "MongoDatabaseSetting", "User"),
        new("CODENAME399_MongoDatabaseSetting__UserProjectMappingCollection", "MongoDatabaseSetting / UserProjectMappingCollection", "MongoDatabaseSetting", "UserProjectMapping"),
        new("CODENAME399_OAuth__CallbackBaseUrl", "OAuth / CallbackBaseUrl", "OAuth", "http://localhost:5001"),
        new("CODENAME399_OAuth__Google__ClientId", "OAuth / Google / ClientId", "OAuth", ""),
        new("CODENAME399_OAuth__Google__ClientSecret", "OAuth / Google / ClientSecret", "OAuth", null, Secret:true),
        new("CODENAME399_OAuth__Google__Enabled", "OAuth / Google / Enabled", "OAuth", "false"),
        new("CODENAME399_OAuth__Microsoft__ClientId", "OAuth / Microsoft / ClientId", "OAuth", ""),
        new("CODENAME399_OAuth__Microsoft__ClientSecret", "OAuth / Microsoft / ClientSecret", "OAuth", null, Secret:true),
        new("CODENAME399_OAuth__Microsoft__Enabled", "OAuth / Microsoft / Enabled", "OAuth", "false"),
        new("CODENAME399_OTPBasedAuthenticationConfig__AccessTokenExpirationHours", "OTPBasedAuthenticationConfig / AccessTokenExpirationHours", "OTPBasedAuthenticationConfig", "1"),
        new("CODENAME399_OTPBasedAuthenticationConfig__Audience", "OTPBasedAuthenticationConfig / Audience", "OTPBasedAuthenticationConfig", "codename399"),
        new("CODENAME399_OTPBasedAuthenticationConfig__Issuer", "OTPBasedAuthenticationConfig / Issuer", "OTPBasedAuthenticationConfig", "codename399"),
        new("CODENAME399_OTPBasedAuthenticationConfig__SecretKey", "OTPBasedAuthenticationConfig / SecretKey", "OTPBasedAuthenticationConfig", null, Secret:true),
        new("CODENAME399_Otp__EmailServiceUrl", "Otp / EmailServiceUrl", "Otp", "http://127.0.0.1:5000"),
        new("CODENAME399_Otp__ExpirationMinutes", "Otp / ExpirationMinutes", "Otp", "10"),
        new("CODENAME399_Otp__Length", "Otp / Length", "Otp", "6"),
        new("CODENAME399_Otp__MaxVerificationAttempts", "Otp / MaxVerificationAttempts", "Otp", "5"),
        new("CODENAME399_Otp__ResendCooldownSeconds", "Otp / ResendCooldownSeconds", "Otp", "60"),
        new("CODENAME399_Otp__Sms__AccountSid", "Otp / Sms / AccountSid", "Otp", ""),
        new("CODENAME399_Otp__Sms__AuthToken", "Otp / Sms / AuthToken", "Otp", null, Secret:true),
        new("CODENAME399_Otp__Sms__Enabled", "Otp / Sms / Enabled", "Otp", "false"),
        new("CODENAME399_Otp__Sms__FromNumber", "Otp / Sms / FromNumber", "Otp", ""),
        new("CODENAME399_PasswordBasedAuthenticationConfig__AccessTokenExpirationHours", "PasswordBasedAuthenticationConfig / AccessTokenExpirationHours", "PasswordBasedAuthenticationConfig", null, Secret:true),
        new("CODENAME399_PasswordBasedAuthenticationConfig__Audience", "PasswordBasedAuthenticationConfig / Audience", "PasswordBasedAuthenticationConfig", null, Secret:true),
        new("CODENAME399_PasswordBasedAuthenticationConfig__Issuer", "PasswordBasedAuthenticationConfig / Issuer", "PasswordBasedAuthenticationConfig", null, Secret:true),
        new("CODENAME399_PasswordBasedAuthenticationConfig__SecretKey", "PasswordBasedAuthenticationConfig / SecretKey", "PasswordBasedAuthenticationConfig", null, Secret:true),
        new("CODENAME399_Smtp__EnableSsl", "Smtp / EnableSsl", "Smtp", "true"),
        new("CODENAME399_Smtp__From", "Smtp / From", "Smtp", ""),
        new("CODENAME399_Smtp__Host", "Smtp / Host", "Smtp", ""),
        new("CODENAME399_Smtp__Password", "Smtp / Password", "Smtp", null, Secret:true),
        new("CODENAME399_Smtp__Port", "Smtp / Port", "Smtp", "587"),
        new("CODENAME399_Smtp__Username", "Smtp / Username", "Smtp", ""),
        new("CODENAME399_TradingConfiguration__BrokerBalanceRefreshSeconds", "TradingConfiguration / BrokerBalanceRefreshSeconds", "TradingConfiguration", "30"),
        new("CODENAME399_TradingConfiguration__BrokerFailureWindowMinutes", "TradingConfiguration / BrokerFailureWindowMinutes", "TradingConfiguration", "2"),
        new("CODENAME399_TradingConfiguration__BrokerPositionConfirmationDelaySeconds", "TradingConfiguration / BrokerPositionConfirmationDelaySeconds", "TradingConfiguration", "1"),
        new("CODENAME399_TradingConfiguration__CapitalAllocationBaseMultiplier", "TradingConfiguration / CapitalAllocationBaseMultiplier", "TradingConfiguration", "0.25"),
        new("CODENAME399_TradingConfiguration__CapitalAllocationConfidenceMultiplier", "TradingConfiguration / CapitalAllocationConfidenceMultiplier", "TradingConfiguration", "0.75"),
        new("CODENAME399_TradingConfiguration__CasEnd", "TradingConfiguration / CasEnd", "TradingConfiguration", "15:40:00"),
        new("CODENAME399_TradingConfiguration__CasLimitOnlyEnd", "TradingConfiguration / CasLimitOnlyEnd", "TradingConfiguration", "15:30:00"),
        new("CODENAME399_TradingConfiguration__CasMarketOnlyEnd", "TradingConfiguration / CasMarketOnlyEnd", "TradingConfiguration", "15:25:00"),
        new("CODENAME399_TradingConfiguration__CasOrderEntryStart", "TradingConfiguration / CasOrderEntryStart", "TradingConfiguration", "15:20:00"),
        new("CODENAME399_TradingConfiguration__CasPostCloseEnd", "TradingConfiguration / CasPostCloseEnd", "TradingConfiguration", "16:00:00"),
        new("CODENAME399_TradingConfiguration__CasPriceBandPercent", "TradingConfiguration / CasPriceBandPercent", "TradingConfiguration", "3"),
        new("CODENAME399_TradingConfiguration__CasRandomCloseSafetyCutoff", "TradingConfiguration / CasRandomCloseSafetyCutoff", "TradingConfiguration", "15:28:00"),
        new("CODENAME399_TradingConfiguration__CasTransitionStart", "TradingConfiguration / CasTransitionStart", "TradingConfiguration", "15:15:00"),
        new("CODENAME399_TradingConfiguration__EliteCapitalBonus", "TradingConfiguration / EliteCapitalBonus", "TradingConfiguration", "0.1"),
        new("CODENAME399_TradingConfiguration__EliteMovementScore", "TradingConfiguration / EliteMovementScore", "TradingConfiguration", "95"),
        new("CODENAME399_TradingConfiguration__EmergencyMarginUtilizationPercent", "TradingConfiguration / EmergencyMarginUtilizationPercent", "TradingConfiguration", "85"),
        new("CODENAME399_TradingConfiguration__EnableEquityTrading", "TradingConfiguration / EnableEquityTrading", "TradingConfiguration", "false"),
        new("CODENAME399_TradingConfiguration__EnableFuturesTrading", "TradingConfiguration / EnableFuturesTrading", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__EnableGlobalRiskLimits", "TradingConfiguration / EnableGlobalRiskLimits", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__EnableLiveTradingPerformanceGate", "TradingConfiguration / EnableLiveTradingPerformanceGate", "TradingConfiguration", "false"),
        new("CODENAME399_TradingConfiguration__EnableOIBuildup", "TradingConfiguration / EnableOIBuildup", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__EnableOptionChainAnalytics", "TradingConfiguration / EnableOptionChainAnalytics", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__EnableOptionsTrading", "TradingConfiguration / EnableOptionsTrading", "TradingConfiguration", "false"),
        new("CODENAME399_TradingConfiguration__EnablePaperMarginSimulation", "TradingConfiguration / EnablePaperMarginSimulation", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__EnablePaperTradingPerformanceGate", "TradingConfiguration / EnablePaperTradingPerformanceGate", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__EnablePutCallRatio", "TradingConfiguration / EnablePutCallRatio", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__EnableScripConsentForCashOrders", "TradingConfiguration / EnableScripConsentForCashOrders", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__Equity__AllowLong", "TradingConfiguration / Equity / AllowLong", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__Equity__AllowShort", "TradingConfiguration / Equity / AllowShort", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__Equity__Duration", "TradingConfiguration / Equity / Duration", "TradingConfiguration", "DAY"),
        new("CODENAME399_TradingConfiguration__Equity__Exchange", "TradingConfiguration / Equity / Exchange", "TradingConfiguration", "NSE"),
        new("CODENAME399_TradingConfiguration__Equity__MaxCapitalPerTrade", "TradingConfiguration / Equity / MaxCapitalPerTrade", "TradingConfiguration", "10000"),
        new("CODENAME399_TradingConfiguration__Equity__MaxMarketDataAgeSeconds", "TradingConfiguration / Equity / MaxMarketDataAgeSeconds", "TradingConfiguration", "15"),
        new("CODENAME399_TradingConfiguration__Equity__MaximumChargesPerTrade", "TradingConfiguration / Equity / MaximumChargesPerTrade", "TradingConfiguration", "100"),
        new("CODENAME399_TradingConfiguration__Equity__MaximumOpenPositions", "TradingConfiguration / Equity / MaximumOpenPositions", "TradingConfiguration", "5"),
        new("CODENAME399_TradingConfiguration__Equity__MaximumPositionsPerUnderlying", "TradingConfiguration / Equity / MaximumPositionsPerUnderlying", "TradingConfiguration", "1"),
        new("CODENAME399_TradingConfiguration__Equity__MaximumSpreadAmount", "TradingConfiguration / Equity / MaximumSpreadAmount", "TradingConfiguration", "5"),
        new("CODENAME399_TradingConfiguration__Equity__MaximumSpreadPercent", "TradingConfiguration / Equity / MaximumSpreadPercent", "TradingConfiguration", "1.5"),
        new("CODENAME399_TradingConfiguration__Equity__MinimumConfidence", "TradingConfiguration / Equity / MinimumConfidence", "TradingConfiguration", "65"),
        new("CODENAME399_TradingConfiguration__Equity__MinimumFinalScore", "TradingConfiguration / Equity / MinimumFinalScore", "TradingConfiguration", "70"),
        new("CODENAME399_TradingConfiguration__Equity__MinimumNetProfit", "TradingConfiguration / Equity / MinimumNetProfit", "TradingConfiguration", "5"),
        new("CODENAME399_TradingConfiguration__Equity__MinimumRoiPercent", "TradingConfiguration / Equity / MinimumRoiPercent", "TradingConfiguration", "0.3"),
        new("CODENAME399_TradingConfiguration__Equity__OrderType", "TradingConfiguration / Equity / OrderType", "TradingConfiguration", "MARKET"),
        new("CODENAME399_TradingConfiguration__Equity__ProductType", "TradingConfiguration / Equity / ProductType", "TradingConfiguration", "INTRADAY"),
        new("CODENAME399_TradingConfiguration__Equity__RiskPercentage", "TradingConfiguration / Equity / RiskPercentage", "TradingConfiguration", "2.0"),
        new("CODENAME399_TradingConfiguration__EquityMisAutoSquareOffTime", "TradingConfiguration / EquityMisAutoSquareOffTime", "TradingConfiguration", "15:10:00"),
        new("CODENAME399_TradingConfiguration__Exit__AtrExitMultiplier", "TradingConfiguration / Exit / AtrExitMultiplier", "TradingConfiguration", "0.4"),
        new("CODENAME399_TradingConfiguration__Exit__MinimumProfitPercent", "TradingConfiguration / Exit / MinimumProfitPercent", "TradingConfiguration", "0.25"),
        new("CODENAME399_TradingConfiguration__Exit__TrailingActivationNetProfit", "TradingConfiguration / Exit / TrailingActivationNetProfit", "TradingConfiguration", "0.0"),
        new("CODENAME399_TradingConfiguration__Exit__TrailingStopAtrMultiplier", "TradingConfiguration / Exit / TrailingStopAtrMultiplier", "TradingConfiguration", "0.6"),
        new("CODENAME399_TradingConfiguration__Futures__AllowLong", "TradingConfiguration / Futures / AllowLong", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__Futures__AllowShort", "TradingConfiguration / Futures / AllowShort", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__Futures__Duration", "TradingConfiguration / Futures / Duration", "TradingConfiguration", "DAY"),
        new("CODENAME399_TradingConfiguration__Futures__Exchange", "TradingConfiguration / Futures / Exchange", "TradingConfiguration", "NFO"),
        new("CODENAME399_TradingConfiguration__Futures__ExpiryType", "TradingConfiguration / Futures / ExpiryType", "TradingConfiguration", "NEAR"),
        new("CODENAME399_TradingConfiguration__Futures__MaxCapitalPerTrade", "TradingConfiguration / Futures / MaxCapitalPerTrade", "TradingConfiguration", "10000"),
        new("CODENAME399_TradingConfiguration__Futures__MaxMarketDataAgeSeconds", "TradingConfiguration / Futures / MaxMarketDataAgeSeconds", "TradingConfiguration", "15"),
        new("CODENAME399_TradingConfiguration__Futures__MaximumChargesPerTrade", "TradingConfiguration / Futures / MaximumChargesPerTrade", "TradingConfiguration", "100"),
        new("CODENAME399_TradingConfiguration__Futures__MaximumDailyLoss", "TradingConfiguration / Futures / MaximumDailyLoss", "TradingConfiguration", "3000"),
        new("CODENAME399_TradingConfiguration__Futures__MaximumDailyTrades", "TradingConfiguration / Futures / MaximumDailyTrades", "TradingConfiguration", "5"),
        new("CODENAME399_TradingConfiguration__Futures__MaximumOpenPositions", "TradingConfiguration / Futures / MaximumOpenPositions", "TradingConfiguration", "3"),
        new("CODENAME399_TradingConfiguration__Futures__MaximumPositionsPerUnderlying", "TradingConfiguration / Futures / MaximumPositionsPerUnderlying", "TradingConfiguration", "1"),
        new("CODENAME399_TradingConfiguration__Futures__MaximumSpreadAmount", "TradingConfiguration / Futures / MaximumSpreadAmount", "TradingConfiguration", "5"),
        new("CODENAME399_TradingConfiguration__Futures__MaximumSpreadPercent", "TradingConfiguration / Futures / MaximumSpreadPercent", "TradingConfiguration", "1.5"),
        new("CODENAME399_TradingConfiguration__Futures__MinimumConfidence", "TradingConfiguration / Futures / MinimumConfidence", "TradingConfiguration", "65"),
        new("CODENAME399_TradingConfiguration__Futures__MinimumFinalScore", "TradingConfiguration / Futures / MinimumFinalScore", "TradingConfiguration", "70"),
        new("CODENAME399_TradingConfiguration__Futures__MinimumNetProfit", "TradingConfiguration / Futures / MinimumNetProfit", "TradingConfiguration", "5"),
        new("CODENAME399_TradingConfiguration__Futures__MinimumOIChangePercent", "TradingConfiguration / Futures / MinimumOIChangePercent", "TradingConfiguration", "0"),
        new("CODENAME399_TradingConfiguration__Futures__MinimumOpenInterest", "TradingConfiguration / Futures / MinimumOpenInterest", "TradingConfiguration", "0"),
        new("CODENAME399_TradingConfiguration__Futures__MinimumRoiPercent", "TradingConfiguration / Futures / MinimumRoiPercent", "TradingConfiguration", "0.3"),
        new("CODENAME399_TradingConfiguration__Futures__OrderType", "TradingConfiguration / Futures / OrderType", "TradingConfiguration", "MARKET"),
        new("CODENAME399_TradingConfiguration__Futures__ProductType", "TradingConfiguration / Futures / ProductType", "TradingConfiguration", "INTRADAY"),
        new("CODENAME399_TradingConfiguration__Futures__RiskPercentage", "TradingConfiguration / Futures / RiskPercentage", "TradingConfiguration", "2.0"),
        new("CODENAME399_TradingConfiguration__FuturesOptionsAutoSquareOffTime", "TradingConfiguration / FuturesOptionsAutoSquareOffTime", "TradingConfiguration", "15:20:00"),
        new("CODENAME399_TradingConfiguration__FuturesOptionsMarketCloseTime", "TradingConfiguration / FuturesOptionsMarketCloseTime", "TradingConfiguration", "15:40:00"),
        new("CODENAME399_TradingConfiguration__IncludeUnrealizedPnlInDailyLoss", "TradingConfiguration / IncludeUnrealizedPnlInDailyLoss", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__IntradayEntryCutoffTime", "TradingConfiguration / IntradayEntryCutoffTime", "TradingConfiguration", "15:20:00"),
        new("CODENAME399_TradingConfiguration__MarketCloseTime", "TradingConfiguration / MarketCloseTime", "TradingConfiguration", "15:30:00"),
        new("CODENAME399_TradingConfiguration__MaxBrokerFailuresBeforeKillSwitch", "TradingConfiguration / MaxBrokerFailuresBeforeKillSwitch", "TradingConfiguration", "5"),
        new("CODENAME399_TradingConfiguration__MaximumMarginUtilizationPercent", "TradingConfiguration / MaximumMarginUtilizationPercent", "TradingConfiguration", "70"),
        new("CODENAME399_TradingConfiguration__MaximumObservedDrawdownPercent", "TradingConfiguration / MaximumObservedDrawdownPercent", "TradingConfiguration", "0.25"),
        new("CODENAME399_TradingConfiguration__MaximumSlippagePercent", "TradingConfiguration / MaximumSlippagePercent", "TradingConfiguration", "0.5"),
        new("CODENAME399_TradingConfiguration__MaximumTotalOpenRisk", "TradingConfiguration / MaximumTotalOpenRisk", "TradingConfiguration", "10000"),
        new("CODENAME399_TradingConfiguration__MaximumTotalUnderlyingDeltaExposure", "TradingConfiguration / MaximumTotalUnderlyingDeltaExposure", "TradingConfiguration", "2000"),
        new("CODENAME399_TradingConfiguration__MinimumLiveTradingConfidence", "TradingConfiguration / MinimumLiveTradingConfidence", "TradingConfiguration", "60"),
        new("CODENAME399_TradingConfiguration__MinimumLiveTradingNetProfit", "TradingConfiguration / MinimumLiveTradingNetProfit", "TradingConfiguration", "0"),
        new("CODENAME399_TradingConfiguration__MinimumLiveTradingPerformanceTrades", "TradingConfiguration / MinimumLiveTradingPerformanceTrades", "TradingConfiguration", "10"),
        new("CODENAME399_TradingConfiguration__MinimumLiveTradingProfitFactor", "TradingConfiguration / MinimumLiveTradingProfitFactor", "TradingConfiguration", "1.2"),
        new("CODENAME399_TradingConfiguration__MinimumLiveTradingRiskReward", "TradingConfiguration / MinimumLiveTradingRiskReward", "TradingConfiguration", "1.5"),
        new("CODENAME399_TradingConfiguration__MinimumLiveTradingWinRate", "TradingConfiguration / MinimumLiveTradingWinRate", "TradingConfiguration", "55"),
        new("CODENAME399_TradingConfiguration__MinimumPaperTradingConfidence", "TradingConfiguration / MinimumPaperTradingConfidence", "TradingConfiguration", "45"),
        new("CODENAME399_TradingConfiguration__MinimumPaperTradingNetProfit", "TradingConfiguration / MinimumPaperTradingNetProfit", "TradingConfiguration", "0"),
        new("CODENAME399_TradingConfiguration__MinimumPaperTradingPerformanceTrades", "TradingConfiguration / MinimumPaperTradingPerformanceTrades", "TradingConfiguration", "3"),
        new("CODENAME399_TradingConfiguration__MinimumPaperTradingProfitFactor", "TradingConfiguration / MinimumPaperTradingProfitFactor", "TradingConfiguration", "0.8"),
        new("CODENAME399_TradingConfiguration__MinimumPaperTradingRiskReward", "TradingConfiguration / MinimumPaperTradingRiskReward", "TradingConfiguration", "1.0"),
        new("CODENAME399_TradingConfiguration__MinimumPaperTradingWinRate", "TradingConfiguration / MinimumPaperTradingWinRate", "TradingConfiguration", "45"),
        new("CODENAME399_TradingConfiguration__MinimumRecentLiveTradingTrades", "TradingConfiguration / MinimumRecentLiveTradingTrades", "TradingConfiguration", "5"),
        new("CODENAME399_TradingConfiguration__NakedRiskMonitorIntervalSeconds", "TradingConfiguration / NakedRiskMonitorIntervalSeconds", "TradingConfiguration", "5"),
        new("CODENAME399_TradingConfiguration__Options__AllowExpiryDayTrading", "TradingConfiguration / Options / AllowExpiryDayTrading", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__Options__AllowLong", "TradingConfiguration / Options / AllowLong", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__Options__AllowNakedCallWriting", "TradingConfiguration / Options / AllowNakedCallWriting", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__Options__AllowNakedPutWriting", "TradingConfiguration / Options / AllowNakedPutWriting", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__Options__AllowNakedStraddle", "TradingConfiguration / Options / AllowNakedStraddle", "TradingConfiguration", "false"),
        new("CODENAME399_TradingConfiguration__Options__AllowNakedStrangle", "TradingConfiguration / Options / AllowNakedStrangle", "TradingConfiguration", "false"),
        new("CODENAME399_TradingConfiguration__Options__AllowNakedWriting", "TradingConfiguration / Options / AllowNakedWriting", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__Options__AllowNakedWritingOnExpiryDay", "TradingConfiguration / Options / AllowNakedWritingOnExpiryDay", "TradingConfiguration", "false"),
        new("CODENAME399_TradingConfiguration__Options__AllowShort", "TradingConfiguration / Options / AllowShort", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__Options__ContractsPerUnderlying", "TradingConfiguration / Options / ContractsPerUnderlying", "TradingConfiguration", "4"),
        new("CODENAME399_TradingConfiguration__Options__Duration", "TradingConfiguration / Options / Duration", "TradingConfiguration", "DAY"),
        new("CODENAME399_TradingConfiguration__Options__EmergencyDeltaExposure", "TradingConfiguration / Options / EmergencyDeltaExposure", "TradingConfiguration", "1000"),
        new("CODENAME399_TradingConfiguration__Options__EmergencyGammaExposure", "TradingConfiguration / Options / EmergencyGammaExposure", "TradingConfiguration", "100"),
        new("CODENAME399_TradingConfiguration__Options__EmergencyIVIncreasePercent", "TradingConfiguration / Options / EmergencyIVIncreasePercent", "TradingConfiguration", "25"),
        new("CODENAME399_TradingConfiguration__Options__EmergencyMarginUtilizationPercent", "TradingConfiguration / Options / EmergencyMarginUtilizationPercent", "TradingConfiguration", "85"),
        new("CODENAME399_TradingConfiguration__Options__EmergencyStressLoss", "TradingConfiguration / Options / EmergencyStressLoss", "TradingConfiguration", "7500"),
        new("CODENAME399_TradingConfiguration__Options__EmergencyVegaExposure", "TradingConfiguration / Options / EmergencyVegaExposure", "TradingConfiguration", "1000"),
        new("CODENAME399_TradingConfiguration__Options__Exchange", "TradingConfiguration / Options / Exchange", "TradingConfiguration", "NFO"),
        new("CODENAME399_TradingConfiguration__Options__ExpiryMarketCloseTime", "TradingConfiguration / Options / ExpiryMarketCloseTime", "TradingConfiguration", "15:40:00"),
        new("CODENAME399_TradingConfiguration__Options__ExpiryType", "TradingConfiguration / Options / ExpiryType", "TradingConfiguration", "NEAR"),
        new("CODENAME399_TradingConfiguration__Options__GreeksCacheSeconds", "TradingConfiguration / Options / GreeksCacheSeconds", "TradingConfiguration", "15"),
        new("CODENAME399_TradingConfiguration__Options__MaxCapitalPerTrade", "TradingConfiguration / Options / MaxCapitalPerTrade", "TradingConfiguration", "10000"),
        new("CODENAME399_TradingConfiguration__Options__MaxMarketDataAgeSeconds", "TradingConfiguration / Options / MaxMarketDataAgeSeconds", "TradingConfiguration", "15"),
        new("CODENAME399_TradingConfiguration__Options__MaximumAbsoluteTheta", "TradingConfiguration / Options / MaximumAbsoluteTheta", "TradingConfiguration", "1000"),
        new("CODENAME399_TradingConfiguration__Options__MaximumChargesPerTrade", "TradingConfiguration / Options / MaximumChargesPerTrade", "TradingConfiguration", "100"),
        new("CODENAME399_TradingConfiguration__Options__MaximumDelta", "TradingConfiguration / Options / MaximumDelta", "TradingConfiguration", "0.8"),
        new("CODENAME399_TradingConfiguration__Options__MaximumExpiryDayIV", "TradingConfiguration / Options / MaximumExpiryDayIV", "TradingConfiguration", "100"),
        new("CODENAME399_TradingConfiguration__Options__MaximumExpiryDayRiskMultiplier", "TradingConfiguration / Options / MaximumExpiryDayRiskMultiplier", "TradingConfiguration", "0.5"),
        new("CODENAME399_TradingConfiguration__Options__MaximumImpliedVolatility", "TradingConfiguration / Options / MaximumImpliedVolatility", "TradingConfiguration", "100"),
        new("CODENAME399_TradingConfiguration__Options__MaximumMarginUtilizationPercent", "TradingConfiguration / Options / MaximumMarginUtilizationPercent", "TradingConfiguration", "70"),
        new("CODENAME399_TradingConfiguration__Options__MaximumNakedOptionLotsPerTrade", "TradingConfiguration / Options / MaximumNakedOptionLotsPerTrade", "TradingConfiguration", "2"),
        new("CODENAME399_TradingConfiguration__Options__MaximumNakedOptionRiskPerTrade", "TradingConfiguration / Options / MaximumNakedOptionRiskPerTrade", "TradingConfiguration", "5000"),
        new("CODENAME399_TradingConfiguration__Options__MaximumNakedStressLossPerTrade", "TradingConfiguration / Options / MaximumNakedStressLossPerTrade", "TradingConfiguration", "5000"),
        new("CODENAME399_TradingConfiguration__Options__MaximumOpenDeltaExposure", "TradingConfiguration / Options / MaximumOpenDeltaExposure", "TradingConfiguration", "1000"),
        new("CODENAME399_TradingConfiguration__Options__MaximumOpenGammaExposure", "TradingConfiguration / Options / MaximumOpenGammaExposure", "TradingConfiguration", "100"),
        new("CODENAME399_TradingConfiguration__Options__MaximumOpenPositions", "TradingConfiguration / Options / MaximumOpenPositions", "TradingConfiguration", "3"),
        new("CODENAME399_TradingConfiguration__Options__MaximumOpenVegaExposure", "TradingConfiguration / Options / MaximumOpenVegaExposure", "TradingConfiguration", "1000"),
        new("CODENAME399_TradingConfiguration__Options__MaximumPositionsPerUnderlying", "TradingConfiguration / Options / MaximumPositionsPerUnderlying", "TradingConfiguration", "1"),
        new("CODENAME399_TradingConfiguration__Options__MaximumPremium", "TradingConfiguration / Options / MaximumPremium", "TradingConfiguration", "100000"),
        new("CODENAME399_TradingConfiguration__Options__MaximumRiskPerUnderlying", "TradingConfiguration / Options / MaximumRiskPerUnderlying", "TradingConfiguration", "2500"),
        new("CODENAME399_TradingConfiguration__Options__MaximumShortLotsPerExpiry", "TradingConfiguration / Options / MaximumShortLotsPerExpiry", "TradingConfiguration", "5"),
        new("CODENAME399_TradingConfiguration__Options__MaximumShortLotsPerStrike", "TradingConfiguration / Options / MaximumShortLotsPerStrike", "TradingConfiguration", "2"),
        new("CODENAME399_TradingConfiguration__Options__MaximumShortLotsPerUnderlying", "TradingConfiguration / Options / MaximumShortLotsPerUnderlying", "TradingConfiguration", "8"),
        new("CODENAME399_TradingConfiguration__Options__MaximumShortPremiumExposure", "TradingConfiguration / Options / MaximumShortPremiumExposure", "TradingConfiguration", "100000"),
        new("CODENAME399_TradingConfiguration__Options__MaximumSpreadAmount", "TradingConfiguration / Options / MaximumSpreadAmount", "TradingConfiguration", "5"),
        new("CODENAME399_TradingConfiguration__Options__MaximumSpreadPercent", "TradingConfiguration / Options / MaximumSpreadPercent", "TradingConfiguration", "1.5"),
        new("CODENAME399_TradingConfiguration__Options__MaximumStrikeCandidatesPerSide", "TradingConfiguration / Options / MaximumStrikeCandidatesPerSide", "TradingConfiguration", "3"),
        new("CODENAME399_TradingConfiguration__Options__MaximumUnderlyingDeltaExposure", "TradingConfiguration / Options / MaximumUnderlyingDeltaExposure", "TradingConfiguration", "750"),
        new("CODENAME399_TradingConfiguration__Options__MaximumUnderlyingStressLoss", "TradingConfiguration / Options / MaximumUnderlyingStressLoss", "TradingConfiguration", "7500"),
        new("CODENAME399_TradingConfiguration__Options__MinimumAsk", "TradingConfiguration / Options / MinimumAsk", "TradingConfiguration", "0"),
        new("CODENAME399_TradingConfiguration__Options__MinimumBid", "TradingConfiguration / Options / MinimumBid", "TradingConfiguration", "0"),
        new("CODENAME399_TradingConfiguration__Options__MinimumCallScore", "TradingConfiguration / Options / MinimumCallScore", "TradingConfiguration", "75"),
        new("CODENAME399_TradingConfiguration__Options__MinimumConfidence", "TradingConfiguration / Options / MinimumConfidence", "TradingConfiguration", "70"),
        new("CODENAME399_TradingConfiguration__Options__MinimumDelta", "TradingConfiguration / Options / MinimumDelta", "TradingConfiguration", "0.25"),
        new("CODENAME399_TradingConfiguration__Options__MinimumFinalScore", "TradingConfiguration / Options / MinimumFinalScore", "TradingConfiguration", "75"),
        new("CODENAME399_TradingConfiguration__Options__MinimumGamma", "TradingConfiguration / Options / MinimumGamma", "TradingConfiguration", "0"),
        new("CODENAME399_TradingConfiguration__Options__MinimumMinutesBeforeExpiry", "TradingConfiguration / Options / MinimumMinutesBeforeExpiry", "TradingConfiguration", "30"),
        new("CODENAME399_TradingConfiguration__Options__MinimumNetProfit", "TradingConfiguration / Options / MinimumNetProfit", "TradingConfiguration", "5"),
        new("CODENAME399_TradingConfiguration__Options__MinimumOIChangePercent", "TradingConfiguration / Options / MinimumOIChangePercent", "TradingConfiguration", "0"),
        new("CODENAME399_TradingConfiguration__Options__MinimumOpenInterest", "TradingConfiguration / Options / MinimumOpenInterest", "TradingConfiguration", "0"),
        new("CODENAME399_TradingConfiguration__Options__MinimumOptionVolume", "TradingConfiguration / Options / MinimumOptionVolume", "TradingConfiguration", "0"),
        new("CODENAME399_TradingConfiguration__Options__MinimumPremium", "TradingConfiguration / Options / MinimumPremium", "TradingConfiguration", "0"),
        new("CODENAME399_TradingConfiguration__Options__MinimumPutScore", "TradingConfiguration / Options / MinimumPutScore", "TradingConfiguration", "75"),
        new("CODENAME399_TradingConfiguration__Options__MinimumRoiPercent", "TradingConfiguration / Options / MinimumRoiPercent", "TradingConfiguration", "0.3"),
        new("CODENAME399_TradingConfiguration__Options__MinimumShortCallScore", "TradingConfiguration / Options / MinimumShortCallScore", "TradingConfiguration", "75"),
        new("CODENAME399_TradingConfiguration__Options__MinimumShortPutScore", "TradingConfiguration / Options / MinimumShortPutScore", "TradingConfiguration", "75"),
        new("CODENAME399_TradingConfiguration__Options__MinimumTurnover", "TradingConfiguration / Options / MinimumTurnover", "TradingConfiguration", "0"),
        new("CODENAME399_TradingConfiguration__Options__MinimumVega", "TradingConfiguration / Options / MinimumVega", "TradingConfiguration", "0"),
        new("CODENAME399_TradingConfiguration__Options__NakedOptionMarginSafetyMultiplier", "TradingConfiguration / Options / NakedOptionMarginSafetyMultiplier", "TradingConfiguration", "1.2"),
        new("CODENAME399_TradingConfiguration__Options__NakedRiskMonitorSeconds", "TradingConfiguration / Options / NakedRiskMonitorSeconds", "TradingConfiguration", "5"),
        new("CODENAME399_TradingConfiguration__Options__NakedStressIVIncreasePercent", "TradingConfiguration / Options / NakedStressIVIncreasePercent", "TradingConfiguration", "10"),
        new("CODENAME399_TradingConfiguration__Options__NakedStressUnderlyingMovePercent", "TradingConfiguration / Options / NakedStressUnderlyingMovePercent", "TradingConfiguration", "3"),
        new("CODENAME399_TradingConfiguration__Options__OptionSide", "TradingConfiguration / Options / OptionSide", "TradingConfiguration", "Both"),
        new("CODENAME399_TradingConfiguration__Options__OrderType", "TradingConfiguration / Options / OrderType", "TradingConfiguration", "MARKET"),
        new("CODENAME399_TradingConfiguration__Options__ProductType", "TradingConfiguration / Options / ProductType", "TradingConfiguration", "INTRADAY"),
        new("CODENAME399_TradingConfiguration__Options__RequireMarketDepth", "TradingConfiguration / Options / RequireMarketDepth", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__Options__RiskPercentage", "TradingConfiguration / Options / RiskPercentage", "TradingConfiguration", "1.0"),
        new("CODENAME399_TradingConfiguration__Options__ShortMaximumDelta", "TradingConfiguration / Options / ShortMaximumDelta", "TradingConfiguration", "0.6"),
        new("CODENAME399_TradingConfiguration__Options__ShortMaximumIV", "TradingConfiguration / Options / ShortMaximumIV", "TradingConfiguration", "100"),
        new("CODENAME399_TradingConfiguration__Options__ShortMaximumPremium", "TradingConfiguration / Options / ShortMaximumPremium", "TradingConfiguration", "100000"),
        new("CODENAME399_TradingConfiguration__Options__ShortMaximumThetaAbs", "TradingConfiguration / Options / ShortMaximumThetaAbs", "TradingConfiguration", "1000"),
        new("CODENAME399_TradingConfiguration__Options__ShortMinimumDelta", "TradingConfiguration / Options / ShortMinimumDelta", "TradingConfiguration", "0.1"),
        new("CODENAME399_TradingConfiguration__Options__ShortMinimumIV", "TradingConfiguration / Options / ShortMinimumIV", "TradingConfiguration", "0"),
        new("CODENAME399_TradingConfiguration__Options__ShortMinimumPremium", "TradingConfiguration / Options / ShortMinimumPremium", "TradingConfiguration", "0"),
        new("CODENAME399_TradingConfiguration__Options__ShortMinimumThetaAbs", "TradingConfiguration / Options / ShortMinimumThetaAbs", "TradingConfiguration", "0"),
        new("CODENAME399_TradingConfiguration__Options__StrikeInterval", "TradingConfiguration / Options / StrikeInterval", "TradingConfiguration", "0"),
        new("CODENAME399_TradingConfiguration__Options__StrikeStepsFromAtm", "TradingConfiguration / Options / StrikeStepsFromAtm", "TradingConfiguration", "10"),
        new("CODENAME399_TradingConfiguration__Options__TradeMode", "TradingConfiguration / Options / TradeMode", "TradingConfiguration", "Both"),
        new("CODENAME399_TradingConfiguration__OrderIntentRecoveryInitialDelaySeconds", "TradingConfiguration / OrderIntentRecoveryInitialDelaySeconds", "TradingConfiguration", "2"),
        new("CODENAME399_TradingConfiguration__OrderIntentRecoveryIntervalSeconds", "TradingConfiguration / OrderIntentRecoveryIntervalSeconds", "TradingConfiguration", "5"),
        new("CODENAME399_TradingConfiguration__OrderIntentUnknownOrderExpiryMinutes", "TradingConfiguration / OrderIntentUnknownOrderExpiryMinutes", "TradingConfiguration", "2"),
        new("CODENAME399_TradingConfiguration__PaperNakedOptionMarginRate", "TradingConfiguration / PaperNakedOptionMarginRate", "TradingConfiguration", "0.03"),
        new("CODENAME399_TradingConfiguration__PaperNakedOptionMarginSafetyMultiplier", "TradingConfiguration / PaperNakedOptionMarginSafetyMultiplier", "TradingConfiguration", "1.2"),
        new("CODENAME399_TradingConfiguration__QuoteMaxTokensPerRequest", "TradingConfiguration / QuoteMaxTokensPerRequest", "TradingConfiguration", "50"),
        new("CODENAME399_TradingConfiguration__QuoteRequestsPerSecond", "TradingConfiguration / QuoteRequestsPerSecond", "TradingConfiguration", "1"),
        new("CODENAME399_TradingConfiguration__RejectDuplicateOrderIntent", "TradingConfiguration / RejectDuplicateOrderIntent", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__RequireBestStrategyMatchForLiveTrading", "TradingConfiguration / RequireBestStrategyMatchForLiveTrading", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__RequireBestStrategyMatchForPaperTrading", "TradingConfiguration / RequireBestStrategyMatchForPaperTrading", "TradingConfiguration", "false"),
        new("CODENAME399_TradingConfiguration__RequireClosedHigherTimeframeCandles", "TradingConfiguration / RequireClosedHigherTimeframeCandles", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__RequirePositiveRecentLiveTradingNetProfit", "TradingConfiguration / RequirePositiveRecentLiveTradingNetProfit", "TradingConfiguration", "true"),
        new("CODENAME399_TradingConfiguration__RiskReservationSeconds", "TradingConfiguration / RiskReservationSeconds", "TradingConfiguration", "10"),
        new("CODENAME399_TradingConfiguration__RoboAutoSquareOffTime", "TradingConfiguration / RoboAutoSquareOffTime", "TradingConfiguration", "15:05:00"),
        new("CODENAME399_TradingConfiguration__SquareOffRetryDelaySeconds", "TradingConfiguration / SquareOffRetryDelaySeconds", "TradingConfiguration", "1"),
        new("CODENAME399_TradingConfiguration__StopLossConfirmationSeconds", "TradingConfiguration / StopLossConfirmationSeconds", "TradingConfiguration", "2"),
        new("CODENAME399_TradingConfiguration__Strategy", "TradingConfiguration / Strategy", "TradingConfiguration", "Pullback"),
        new("CODENAME399_TradingConfiguration__StrongCapitalBonus", "TradingConfiguration / StrongCapitalBonus", "TradingConfiguration", "0.05"),
        new("CODENAME399_TradingConfiguration__StrongMovementScore", "TradingConfiguration / StrongMovementScore", "TradingConfiguration", "90"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__0", "TradingConfiguration / TradingHolidays [0]", "TradingConfiguration", "2026-01-26"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__1", "TradingConfiguration / TradingHolidays [1]", "TradingConfiguration", "2026-02-19"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__2", "TradingConfiguration / TradingHolidays [2]", "TradingConfiguration", "2026-03-03"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__3", "TradingConfiguration / TradingHolidays [3]", "TradingConfiguration", "2026-03-19"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__4", "TradingConfiguration / TradingHolidays [4]", "TradingConfiguration", "2026-03-26"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__5", "TradingConfiguration / TradingHolidays [5]", "TradingConfiguration", "2026-03-31"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__6", "TradingConfiguration / TradingHolidays [6]", "TradingConfiguration", "2026-04-01"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__7", "TradingConfiguration / TradingHolidays [7]", "TradingConfiguration", "2026-04-03"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__8", "TradingConfiguration / TradingHolidays [8]", "TradingConfiguration", "2026-04-14"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__9", "TradingConfiguration / TradingHolidays [9]", "TradingConfiguration", "2026-05-01"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__10", "TradingConfiguration / TradingHolidays [10]", "TradingConfiguration", "2026-05-28"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__11", "TradingConfiguration / TradingHolidays [11]", "TradingConfiguration", "2026-06-26"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__12", "TradingConfiguration / TradingHolidays [12]", "TradingConfiguration", "2026-08-26"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__13", "TradingConfiguration / TradingHolidays [13]", "TradingConfiguration", "2026-09-14"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__14", "TradingConfiguration / TradingHolidays [14]", "TradingConfiguration", "2026-10-02"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__15", "TradingConfiguration / TradingHolidays [15]", "TradingConfiguration", "2026-10-20"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__16", "TradingConfiguration / TradingHolidays [16]", "TradingConfiguration", "2026-11-10"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__17", "TradingConfiguration / TradingHolidays [17]", "TradingConfiguration", "2026-11-24"),
        new("CODENAME399_TradingConfiguration__TradingHolidays__18", "TradingConfiguration / TradingHolidays [18]", "TradingConfiguration", "2026-12-25"),
        new("CODENAME399_TradingConfiguration__VirtualTrading__AboveEntryRatioWeight", "TradingConfiguration / VirtualTrading / AboveEntryRatioWeight", "TradingConfiguration", "30"),
        new("CODENAME399_TradingConfiguration__VirtualTrading__EntryMinimumPriceRatio", "TradingConfiguration / VirtualTrading / EntryMinimumPriceRatio", "TradingConfiguration", "0.998"),
        new("CODENAME399_TradingConfiguration__VirtualTrading__FallbackAtrPercent", "TradingConfiguration / VirtualTrading / FallbackAtrPercent", "TradingConfiguration", "0.5"),
        new("CODENAME399_TradingConfiguration__VirtualTrading__HighestPriceMinimumRatio", "TradingConfiguration / VirtualTrading / HighestPriceMinimumRatio", "TradingConfiguration", "0.997"),
        new("CODENAME399_TradingConfiguration__VirtualTrading__MaximumConsecutivePositiveBonus", "TradingConfiguration / VirtualTrading / MaximumConsecutivePositiveBonus", "TradingConfiguration", "15"),
        new("CODENAME399_TradingConfiguration__VirtualTrading__MaximumDrawdownPercent", "TradingConfiguration / VirtualTrading / MaximumDrawdownPercent", "TradingConfiguration", "0.5"),
        new("CODENAME399_TradingConfiguration__VirtualTrading__MaximumHigherHighBonus", "TradingConfiguration / VirtualTrading / MaximumHigherHighBonus", "TradingConfiguration", "15"),
        new("CODENAME399_TradingConfiguration__VirtualTrading__PositiveRatioWeight", "TradingConfiguration / VirtualTrading / PositiveRatioWeight", "TradingConfiguration", "40"),
        new("CODENAME399_TradingConfiguration__VirtualTrading__VolatilityHighThreshold", "TradingConfiguration / VirtualTrading / VolatilityHighThreshold", "TradingConfiguration", "1.2"),
        new("CODENAME399_TradingConfiguration__VirtualTrading__VolatilityLowThreshold", "TradingConfiguration / VirtualTrading / VolatilityLowThreshold", "TradingConfiguration", "0.5"),
        new("CODENAME399_TradingConfiguration__VirtualTrading__VolatilityMediumThreshold", "TradingConfiguration / VirtualTrading / VolatilityMediumThreshold", "TradingConfiguration", "0.8"),
        new("CODENAME399_TradingConfiguration__VirtualTrading__VolatilityVeryHighThreshold", "TradingConfiguration / VirtualTrading / VolatilityVeryHighThreshold", "TradingConfiguration", "2.0"),
        new("CODENAME399_TradingConfiguration__VirtualTrading__VolatilityVeryLowThreshold", "TradingConfiguration / VirtualTrading / VolatilityVeryLowThreshold", "TradingConfiguration", "0.2"),
        new("CODENAME399_TradingConfiguration__WebSocketHeartbeatSeconds", "TradingConfiguration / WebSocketHeartbeatSeconds", "TradingConfiguration", "10"),
        new("CODENAME399_TradingConfiguration__WebSocketPongTimeoutSeconds", "TradingConfiguration / WebSocketPongTimeoutSeconds", "TradingConfiguration", "30"),
        new("CODENAME399_TradingConfiguration__WebSocketRetryInitialSeconds", "TradingConfiguration / WebSocketRetryInitialSeconds", "TradingConfiguration", "10"),
        new("CODENAME399_TradingConfiguration__WebSocketRetryMaxSeconds", "TradingConfiguration / WebSocketRetryMaxSeconds", "TradingConfiguration", "60"),
        new("CODENAME399_TradingRuntime__InstrumentType", "TradingRuntime / InstrumentType", "TradingRuntime", "Futures")
    };
    readonly Dictionary<string, TextBox> fields = new();
    readonly Dictionary<string, Label> status = new();
    readonly RichTextBox log = new();
    readonly Label summary = new();
    readonly Button setMissing = new(), saveAll = new(), refresh = new();

    public MainForm()
    {
        Text = "CodeName399 Environment Manager";
        Width = 1400;
        Height = 900;
        MinimumSize = new Size(1100, 650);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9F);

        var header = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 92,
            ColumnCount = 1,
            Padding = new Padding(12)
        };

        header.Controls.Add(new Label
        {
            Text = "CodeName399 Environment Manager",
            Font = new Font(Font.FontFamily, 16, FontStyle.Bold),
            AutoSize = true
        });

        header.Controls.Add(new Label
        {
            Text = "Environment overrides for Auth, Gateway, EquityTrading, FutureTrading and OptionsTrading",
            AutoSize = true
        });

        summary.Text = "Loading...";
        summary.AutoSize = true;
        header.Controls.Add(summary);

        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            Padding = new Padding(10)
        };

        var listHost = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(0, 0, 8, 0)
        };

        var list = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 6,
            Padding = new Padding(4),
            GrowStyle = TableLayoutPanelGrowStyle.AddRows
        };

        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 290));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 88));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96));
        list.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 86));

        string? lastCategory = null;

        void AddCell(Control c, int col, int row) => list.Controls.Add(c, col, row);

        AddCell(new Label { Text = "Variable", Font = new Font(Font, FontStyle.Bold), Dock = DockStyle.Fill, Padding = new Padding(4, 8, 0, 0) }, 0, 0);
        AddCell(new Label { Text = "Value", Font = new Font(Font, FontStyle.Bold), Dock = DockStyle.Fill, Padding = new Padding(4, 8, 0, 0) }, 1, 0);
        AddCell(new Label { Text = "Status", Font = new Font(Font, FontStyle.Bold), Dock = DockStyle.Fill, Padding = new Padding(4, 8, 0, 0) }, 2, 0);
        AddCell(new Label { Text = "Save", Font = new Font(Font, FontStyle.Bold), Dock = DockStyle.Fill, Padding = new Padding(4, 8, 0, 0) }, 3, 0);
        AddCell(new Label { Text = "Delete", Font = new Font(Font, FontStyle.Bold), Dock = DockStyle.Fill, Padding = new Padding(4, 8, 0, 0) }, 4, 0);
        AddCell(new Label { Text = "Show", Font = new Font(Font, FontStyle.Bold), Dock = DockStyle.Fill, Padding = new Padding(4, 8, 0, 0) }, 5, 0);
        list.RowCount = 1;

        foreach (var item in items)
        {
            if (item.Category != lastCategory)
            {
                var row = list.RowCount;
                list.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

                var cat = new Label
                {
                    Text = item.Category,
                    Font = new Font(Font, FontStyle.Bold),
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    Padding = new Padding(3, 7, 0, 0)
                };

                list.Controls.Add(cat, 0, row);
                list.SetColumnSpan(cat, 6);
                list.RowCount++;
                lastCategory = item.Category;
            }

            var currentRow = list.RowCount;

            var variable = new Label
            {
                Text = item.Name,
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(4, 8, 4, 4)
            };

            var tb = new TextBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(3, 4, 3, 4),
                UseSystemPasswordChar = item.Secret,
                Tag = item
            };

            fields[item.Name] = tb;

            var st = new Label
            {
                Text = "...",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(4, 8, 4, 4)
            };

            status[item.Name] = st;

            var save = new Button
            {
                Text = "Save",
                Dock = DockStyle.Fill,
                Height = 30,
                Tag = item,
                Margin = new Padding(3)
            };

            var del = new Button
            {
                Text = "Delete",
                Dock = DockStyle.Fill,
                Height = 30,
                Tag = item,
                Margin = new Padding(3)
            };

            save.Click += async (_, _) => await SaveOneAsync(item);
            del.Click += async (_, _) => await DeleteOneAsync(item);

            var reveal = new CheckBox
            {
                Text = "Show",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(4, 6, 2, 3),
                Enabled = item.Secret
            };

            reveal.CheckedChanged += (_, _) =>
            {
                if (item.Secret)
                    tb.UseSystemPasswordChar = !reveal.Checked;
            };

            list.Controls.Add(variable, 0, currentRow);
            list.Controls.Add(tb, 1, currentRow);
            list.Controls.Add(st, 2, currentRow);
            list.Controls.Add(save, 3, currentRow);
            list.Controls.Add(del, 4, currentRow);
            list.Controls.Add(reveal, 5, currentRow);
            list.RowCount++;
        }

        listHost.Controls.Add(list);
        split.Panel1.Controls.Add(listHost);

        var right = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Padding = new Padding(8, 0, 0, 0)
        };

        right.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            WrapContents = true,
            AutoScroll = true,
            Padding = new Padding(0, 4, 0, 4)
        };

        ConfigureButton(setMissing, "Set Missing", async (_, _) => await SetMissingAsync());
        ConfigureButton(saveAll, "Save All", async (_, _) => await SaveAllAsync());
        ConfigureButton(refresh, "Refresh", async (_, _) => await RefreshAsync());

        actions.Controls.Add(setMissing);
        actions.Controls.Add(saveAll);
        actions.Controls.Add(refresh);
        right.Controls.Add(actions, 0, 0);

        log.Dock = DockStyle.Fill;
        log.ReadOnly = true;
        log.BackColor = Color.FromArgb(20, 22, 26);
        log.ForeColor = Color.Gainsboro;
        log.Font = new Font("Consolas", 9);
        right.Controls.Add(log, 0, 1);

        split.Panel2.Controls.Add(right);
        Controls.Add(split);
        Controls.Add(header);

        void SetSafeSplitterDistance()
        {
            if (split.ClientSize.Width <= 0)
                return;

            const int panel1Min = 700;
            const int panel2Min = 300;

            if (split.ClientSize.Width < panel1Min + panel2Min + split.SplitterWidth)
                return;

            split.Panel1MinSize = panel1Min;
            split.Panel2MinSize = panel2Min;

            var usable = split.ClientSize.Width;
            var max = usable - split.Panel2MinSize - split.SplitterWidth;
            var distance = Math.Clamp((int)(usable * 0.62), split.Panel1MinSize, max);

            if (distance >= split.Panel1MinSize && distance <= max)
                split.SplitterDistance = distance;
        }

        Shown += (_, _) => BeginInvoke(SetSafeSplitterDistance);
        SizeChanged += (_, _) =>
        {
            if (IsHandleCreated)
                BeginInvoke(SetSafeSplitterDistance);
        };
        Shown += async (_, _) => await RefreshAsync();
    }

    void ConfigureButton(Button button, string text, EventHandler handler)
    {
        button.Text = text;
        button.AutoSize = true;
        button.Height = 34;
        button.MinimumSize = new Size(105, 34);
        button.Click += handler;
    }

    string? GetMachine(string name) =>
        Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);

    void SetMachine(string name, string value) =>
        Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.Machine);

    void WriteLog(string message, bool error = false)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => WriteLog(message, error));
            return;
        }

        log.AppendText($"{DateTime.Now:HH:mm:ss} {(error ? "[ERROR] " : "")}{message}{Environment.NewLine}");
        log.SelectionStart = log.TextLength;
        log.ScrollToCaret();
    }

    async Task SaveOneAsync(EnvItem item)
    {
        try
        {
            var value = fields[item.Name].Text.Trim();

            if (item.Required && string.IsNullOrWhiteSpace(value))
            {
                WriteLog($"SKIP    {item.Name} - empty required value", true);
                MessageBox.Show($"Enter a value for {item.Label}.", "Save Variable",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetMachine(item.Name, value);
            WriteLog($"SAVED   {item.Name}");
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            WriteLog($"ERROR   {item.Name}: {ex.Message}", true);
            MessageBox.Show(ex.Message, "Save Variable Failed",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    async Task DeleteOneAsync(EnvItem item)
    {
        if (MessageBox.Show(
                $"Delete the machine-level environment variable '{item.Name}'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        try
        {
            Environment.SetEnvironmentVariable(item.Name, null, EnvironmentVariableTarget.Machine);
            WriteLog($"DELETED {item.Name}");
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            WriteLog($"ERROR   deleting {item.Name}: {ex.Message}", true);
            MessageBox.Show(ex.Message, "Delete Variable Failed",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    async Task RefreshAsync()
    {
        await Task.Run(() =>
        {
            foreach (var item in items)
            {
                var value = GetMachine(item.Name);

                BeginInvoke(() =>
                {
                    fields[item.Name].Text = value ?? item.DefaultValue ?? "";
                    status[item.Name].Text =
                        string.IsNullOrWhiteSpace(value)
                            ? (item.DefaultValue == null ? "NOT SET" : "DEFAULT")
                            : "EXISTS";
                });
            }
        });

        var set = items.Count(i => !string.IsNullOrWhiteSpace(GetMachine(i.Name)));
        var defaults = items.Count(i =>
            string.IsNullOrWhiteSpace(GetMachine(i.Name)) &&
            i.DefaultValue != null);

        summary.Text = $"{items.Count} variables • {set} explicitly set • {defaults} using appsettings defaults";
        WriteLog("Environment status refreshed.");
    }

    async Task SetMissingAsync()
    {
        setMissing.Enabled = saveAll.Enabled = refresh.Enabled = false;

        try
        {
            foreach (var item in items)
            {
                if (!string.IsNullOrWhiteSpace(GetMachine(item.Name)))
                {
                    WriteLog($"EXISTS  {item.Name}");
                    continue;
                }

                var value = fields[item.Name].Text.Trim();

                if (string.IsNullOrWhiteSpace(value))
                {
                    if (item.DefaultValue != null)
                        value = item.DefaultValue;
                    else
                    {
                        WriteLog($"SKIP    {item.Name} - no value supplied");
                        continue;
                    }
                }

                SetMachine(item.Name, value);
                WriteLog($"SET     {item.Name}");
            }

            MessageBox.Show(
                "Missing values were set where a value/default was available. Existing values were preserved.",
                "Environment Setup",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            WriteLog("ERROR: " + ex.Message, true);
            MessageBox.Show(ex.Message, "Environment Setup Failed",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            setMissing.Enabled = saveAll.Enabled = refresh.Enabled = true;
            await RefreshAsync();
        }
    }

    async Task SaveAllAsync()
    {
        if (MessageBox.Show(
                "Save all visible values to machine-level environment variables? Existing values will be overwritten.",
                "Confirm Save All",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        setMissing.Enabled = saveAll.Enabled = refresh.Enabled = false;

        try
        {
            foreach (var item in items)
            {
                var value = fields[item.Name].Text.Trim();

                if (item.Required && string.IsNullOrWhiteSpace(value))
                {
                    WriteLog($"SKIP    {item.Name} - empty required value", true);
                    continue;
                }

                SetMachine(item.Name, value);
                WriteLog($"SAVED   {item.Name}");
            }

            MessageBox.Show("Environment variables saved.", "Environment Setup",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            WriteLog("ERROR: " + ex.Message, true);
            MessageBox.Show(ex.Message, "Save Failed",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            setMissing.Enabled = saveAll.Enabled = refresh.Enabled = true;
            await RefreshAsync();
        }
    }
}

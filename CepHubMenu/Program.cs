using CepHub.Services;
using CepHub.Utils;
using CepHub.View;

var httpClient = new HttpClient();
var logger = new Logger();
var normalizer = new CepNormalizer();

var cepService = new CepService(httpClient, logger, normalizer);

var menu = new Menu(cepService, logger);

await menu.Run();
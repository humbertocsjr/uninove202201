
# Projeto VagasFast

Feito em DotNet 6.0 na linguagem C# usando tecnologias MAUI com adendo Blazor, WebAPI, integrado ao serviço BingMaps, compilável em iOS/Android/Desktop macOS.

Desenvolvido usando Visual Studio for Mac/Xcode/Android SDK

# Conceito

Um aplicativo de aluguel de vagas excedentes de garagem, onde o dono da vaga anuncia e tem sua vaga vistoriada e disponibilizada no aplicativo, onde é visto pelos usuários que podem se cadastrar e solicitar a reserva da vaga.

# Execução

Para executar precisa-se do Visual Studio e/ou dotNet 6.0 SDK instalado com o Android SDK ou Xcode para compilação, bastando alterar o arquivo /Data/Api.cs no aplicativo para apontar para o endereço do servidor, e executar o servidor simultaneamente.

Utiliza sqlLite como banco de dados do lado servidor.

Por o servidor e cliente rodarem em ambiente de desenvolvimento precisa verificar as permissoes de acesso a rede do Emulador de Dispositivo do Android ou Simulador de iOS do Xcode para que possa haver conexão entre os dois, além do firewall da maquina de desenvolvimento, para não dar falha de conexão ao tentar se cadastrar, fazer login e solicitar vagas, rodando em ambiente de produção não precisa de todas essas verificações.

Testado em um iPhone XS Max conectado diretamente via Xcode, Emulador de Pixel 5, Simulador de iPhone 13
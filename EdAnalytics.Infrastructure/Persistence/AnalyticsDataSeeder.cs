using EdAnalytics.Domain;
using System.Text;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace EdAnalytics.Infrastructure.Persistence
{
    public static class AnalyticsDataSeeder
    {
        public static void Initialize(AnalyticsDbContext context)
        {
            try
            {
                var titulosSemente = new List<string> {
                    "Como é Sofrido ser Corinthiano: Guia Definitivo",
                    "Engenharia de Dados com C# e EF Core",
                    "A História e o Paradoxo do EAD",
                    "Design System em Plataformas Web",
                    "Gestão Ágil: Como não enlouquecer com código"
                };

                // Sempre limpamos para a última versão criativa injetada
                var antigos = context.Cursos.Where(c => titulosSemente.Contains(c.Titulo)).ToList();
                if (antigos.Count > 0)
                {
                    context.Cursos.RemoveRange(antigos);
                    context.SaveChanges();
                }

                Console.WriteLine("[SEEDER] Criando novos Cursos com conteúdo literário...");
                var cursos = new List<Curso>
                {
                    new Curso { Titulo = "Como é Sofrido ser Corinthiano: Guia Definitivo", Area = "Filosofia e Dor", Visualizacoes = 9999 },
                    new Curso { Titulo = "Engenharia de Dados com C# e EF Core", Area = "Tecnologia", Visualizacoes = 150 },
                    new Curso { Titulo = "A História e o Paradoxo do EAD", Area = "Educação", Visualizacoes = 400 },
                    new Curso { Titulo = "Design System em Plataformas Web", Area = "Design", Visualizacoes = 80 },
                    new Curso { Titulo = "Gestão Ágil: Como não enlouquecer com código", Area = "Negócios", Visualizacoes = 500 }
                };

                context.Cursos.AddRange(cursos);
                context.SaveChanges();

                var aulas = new List<Aula>();

                foreach (var curso in cursos)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        string tituloAula = $"Módulo {i}: {GetTituloModulo(curso.Titulo, i)}";
                        string conteudoReal = GetConteudoAula(curso.Titulo, i);

                        StringBuilder htmlContent = new StringBuilder();
                        htmlContent.AppendLine($"<h2>{tituloAula}</h2>");
                        htmlContent.AppendLine("<hr />");
                        htmlContent.AppendLine($"<div class='mt-4 fs-5' style='line-height: 1.8;'>{conteudoReal}</div>");

                        aulas.Add(new Aula
                        {
                            Titulo = tituloAula,
                            Conteudo = htmlContent.ToString(),
                            CursoId = curso.Id
                        });
                    }
                }

                context.Aulas.AddRange(aulas);
                context.SaveChanges();
                Console.WriteLine("[SEEDER] Aulas gravadas com sucesso no Oracle!");
            }
            catch(Exception ex)
            {
                Console.WriteLine("================= ERRO GRAVE NO SEEDER =====================");
                Console.WriteLine(ex.ToString());
            }
        }

        private static string GetTituloModulo(string curso, int modulo)
        {
            if (curso.Contains("Corinthiano"))
            {
                if (modulo == 1) return "A Gênese do Sofrimento e os 23 Anos de Tabu";
                if (modulo == 2) return "A Esperança como Punição, de Tóquio à Série B";
                return "A Catarse da Vitória: Cássio e a Imortalidade";
            }
            if (curso.Contains("Engenharia"))
            {
                if (modulo == 1) return "O Paradigma ORM: Como o EF Core lê sua alma";
                if (modulo == 2) return "O Terror das Migrations em Ambiente de Produção";
                return "Performance Extremada e Tracking Invisível";
            }
            if (curso.Contains("Paradoxo"))
            {
                if (modulo == 1) return "Do Instituto Universal à Fibra Ótica";
                if (modulo == 2) return "A Solidão do Aluno Atrás da Webcam Desligada";
                return "Dopamina Virtual e o Futuro Híbrido";
            }
            if (curso.Contains("Design"))
            {
                if (modulo == 1) return "Por Que Aquele Botão Está 2px Fora do Lugar?";
                if (modulo == 2) return "A Tirania do HEX: Variáveis que Salvam Vidas";
                return "Guerras no Figma: Dev vs Designer";
            }
            if (curso.Contains("Gestão Ágil"))
            {
                if (modulo == 1) return "O Fim da Cascata e a Ascensão do Caos Controlado";
                if (modulo == 2) return "As Dailies que Duram Meia Hora (Um Delírio Coletivo)";
                return "Story Points Não São Horas: O Paradoxo de Fibonacci";
            }
            return $"Tópico Padrão {modulo}";
        }

        private static string GetConteudoAula(string curso, int modulo)
        {
            if (curso.Contains("Corinthiano"))
            {
                if (modulo == 1) return @"
<p>Ser corinthiano nunca foi apenas uma escolha esportiva; é a adesão voluntária a um modo de vida repleto de angústia crônica e fé inabalável. Nesta primeira aula, mergulhamos nas raízes neurológicas da teimosia que forjou a paixão da Fiel torcida. Retornamos à noite histórica de 1977, quando o sofrimento de um jejum quase interminável de 23 anos encontrou o pé iluminado de Basílio.</p>
<p>Aqueles 23 anos criaram uma anomalia esportiva: em vez de abandonar o barco, o torcedor transformou o sofrimento em uma liturgia religiosa. O corinthiano é o único fã de futebol que sente um conforto estranho quando o time sofre um gol logo aos 5 minutos do primeiro tempo; a sensação não é de desespero, mas sim: 'Agora sim, as coisas estão normais, podemos começar a torcer'.</p>
<p>Exploramos a relação simbiótica entre o frio desalmado daquela região leste de São Paulo, no bairro de Itaquera, e o coração febril daqueles que pagam o preço de vestir a camisa alvinegra. É mais do que bater palma; é sobre estar presente quando até mesmo a lógica parece mandar todos embora para casa.</p>";
                if (modulo == 2) return @"
<p>Dizem que a esperança é a última que morre, mas no universo alvinegro ela é geralmente a primeira que te faz sofrer. Por que a esperança pune tanto? 'Vai Corinthians' é muito mais do que um brado ensurdecedor; é um amparo e um escudo protetor contra o mundo lá fora.</p>
<p>Discutimos o fatídico ano de 2007. A desgraça do rebaixamento, aquela sensação terrível de fim da picada. O mundo do futebol projetava uma implosão para sempre, mas em vez disso, nasceu o movimento 'Eu Nunca Vou Te Abandonar'. A dor de cair para a Série B pavimentou a estrada de tijolos amarelos rumo aos maiores triunfos que o time viveria em sua existência. Essa lição serve não apenas para o gramado, mas para a resiliência no dia a dia da vida civil.</p>
<p>Não confie em um Corinthians franco favorito; ali mora a zica. Acredite no Corinthians que precisa de uma combinação improvável de três empates nos outros jogos, um gol roubado no minuto 49 e um goleiro com câimbra salvando a bola numa unha de distância da linha de gol. Isso é espetáculo, isso é cinema puro na arquibancada com sinalizadores.</p>";
                return @"
<p>E quando todo esse inferno mental acaba dando frutos, chegamos à catarse total. O sentimento que preenche o peito do corinthiano no exato milissegundo de uma defesa impossível de mão trocada. Em 2012 assistimos Cássio virar um semideus ao interceptar um chute de Diego Souza que já contava com a certeza da morte na Libertadores.</p>
<p>A explosão de sentimentos transcende a lógica e o espaço físico de uma cidade. Quando Romarinho, no alto da mais pura irresponsabilidade juvenil que só um gênio pode carregar no peito, cavou a bola por cima do goleiro com cara de choro na mítica Bombonera... aquele, sem sombra de dúvidas, foi o dia em que milhares de sofrimentos passados e estragos cardiovasculares foram oficialmente curados e quitados.</p>
<p>O grito de libertação faz cada tropeço das décadas anteriores valer a pena na conta bancária do destino. Um corinthiano feliz não teme o dia seguinte, ele flutua. O coração, embora bastante machucado e propenso a infartos precoces, é um coração campeão de fato e de alma. Vai Corinthians, ontem, hoje, amanhã e por causa do seu coração, sempre!</p>";
            }
            if (curso.Contains("Gestão Ágil"))
            {
                if (modulo == 1) return @"
<p>O desenvolvimento de software no início dos anos 2000 vivia na sombra do assustador e burocrático Método Cascata – no qual os requisitos tomavam seis meses e a falha de um botão invalidava três anos de esforços sem contato com o cliente.</p>
<p>A ascensão do Manifesto Ágil propôs o Caos Controlado, a Iteração Mágica. Neste módulo prático, analisamos a beleza por trás das metodologias enxutas (Leans). Descubra por que a agilidade não se trata primariamente de ser 'rápido', mas trata de não enlouquecer o seu time inteiro no meio do processo criando coisas que ninguém quer. Falhar rápido custa centavos em vez de milhões, e responder às mudanças virou algo mais majestoso do que assinar contratos rígidos repletos de entrelinhas.</p>";
                if (modulo == 2) return @"
<p>É uma das maiores pragas de um projeto mal direcionado: as famosas reuniões diárias (Dailies). Foi combinadinho que era para durar estritamente 15 minutos em pé, então me explica o porquê de os devs ficarem parados por 45 minutos debatendo o framework CSS enquanto ninguém ouvia?</p>
<p>Nesta análise pesada sobre os anti-padrões, aprofundamos nos motivos psicológicos que fazem a equipe arrastar as plannings por horas sem fim. Revelamos as tristes realidades entre as estimativas que o desenvolvedor dá em um belo dia otimista versus aquilo que o mundo real e um bug na API de pagamentos exigirão no final do mês sem piedade.</p>";
                return @"
<p>Story Points definitivamente não são representações de horas fechadas ou relógios esgotando, mas não chame um Stakeholder num café para explicar isso a menos que você esteja pronto para ouvir o contrário com grosseria elegante. Mas brincadeiras à parte, entramos no verdadeiro paradoxo da série de Fibonacci usada pelos Scrum Masters modernos.</p>
<p>Trabalhar o burnout da equipe, entender quando limpar o seu Kanban para focar puramente em dívidas técnicas do código sujo de meses atrás e como manter o sorriso no rosto quando o servidor cair de quarta para quinta-feira é o verdadeiro ensinamento mágico deste treinamento profundo.</p>";
            }
            if (curso.Contains("Engenharia"))
            {
                if (modulo == 1) return @"
<p>O que acontece quando você escreve uma expressão LINQ ingênua no Visual Studio e o Entity Framework traduz isso para o banco de dados? Nesta introdução fantástica sobre os perigos atrelados a um Object-Relational Mapper (ORM), investigamos como o EF Core facilita e simultaneamente aterroriza nossa sanidade.</p>
<p>O fatídico conflito que chamamos tecnicamente de N+1 queries – onde de repente uma solicitação HTTP dispara 50 idas na máquina do servidor do banco em décimos de segundos, colocando o gargalo em seu teto limite. O ORM é mágico, mas cobra a prudência de desenvolvedores como um pedágio.</p>";
                if (modulo == 2) return @"
<p>O coração do DBA até treme quando vê a palavra mágico 'Snapshot' e 'Migration'. Ao enviar uma alteração de tabela na sexta-feira perto das seis horas da tarde, as histórias que escutamos viram lendas na empresa.</p>
<p>Explicamos as estratégias para rodar essas atualizações no banco sem travar toda a tabela da aplicação no banco de produção. A criação de scripts idempotentes e revisões seguras. É um mergulho profundo nas responsabilidades críticas de fazer grandes modificações arquiteturais de Dados na ponta macia do mouse.</p>";
                return @"
<p>O estado silencioso de Change Tracking consome a sua memória RAM sem dó nem piedade numa requisição simples de listar itens. É exatamente sobre domínios como o AsNoTracking(), e na introdução brilhante de Compiled Queries em cenários hostis de alta escala computacional, que moldamos esse capítulo focado na Performance Crítica da Cloud.</p>";
            }
            if (curso.Contains("Paradoxo"))
            {
                if (modulo == 1) return @"
<p>A romantização da Educação a Distância (EAD) sofreu grandes transformações ao longo das eras na civilização recente. Começamos com remessas de cursos via caixa postal dos jornais, entregando apostilas em lares rurais desde meados do século anterior. Pulos evolutivos nos trouxeram ao ápice efervescente da Fibra Óptica, da nuvem e do stream no protocolo HTML5 sem travas.</p>
<p>Ao observar os limites de engajamento do antigo tutor que não se conectava frente a frente, comparamos como os conceitos pedagógicos tradicionais quebraram durante a massificação das telas frias no ensino básico e universitário de grande proporção.</p>";
                if (modulo == 2) return @"
<p>Um fenômeno puramente digital que ataca com força assombrosa: A Solidão Atrás da Webcam Desligada. O que o distanciamento total das dinâmicas escolares tem de reflexo prático sobre a proatividade e as taxas terríveis de evasão no meio acadêmico da Internet?</p>
<p>O dilema existencial dos monitores implorando ao vivo 'alguém responde minha pergunta no microfone e não no chat, pessoal?' reflete a frieza dos tempos tecnológicos sem as soft skills presencias da empatia interpessoal.</p>";
                return @"
<p>Para recuperar mentes que se dissipam fáceis com redes sociais paralelas aos estudos, o EAD entra na fase mágica da hiper-gamificação. Loops artificiais e injeções de dopamina no Moodle viraram táticas cruciais, as badges motivam, transformam módulos longos insuportáveis em missões vitais dignas de RPGs modernos focados em reter o cérebro humano por muito mais do que seria fisicamente natural.</p>";
            }
            if (curso.Contains("Design"))
            {
                if (modulo == 1) return @"
<p>O design contemporâneo de interface repudia a total e absoluta inconsistência! Por que um botão de confirmar de um menu tem que estar dois gloriosos pixels descascados foras da ordem geométrica nos mobile forms? Neste módulo base entendemos como os olhos capturam inconsistências antes do cérebro racionalizar qual foi o estrago da má forma.</p>
<p>A criação da espinha dorsal semântica evita desgastes pesados de escalabilidade na medida em que a Plataforma alcança patamares continentais.</p>";
                if (modulo == 2) return @"
<p>Sair escrevendo valores de HEX hardcodeds espalhados pelas folhas em cascatas no código fonte do front é o primeiro prego num caixão da usabilidade. Você aprende nesta sessão a ditadura do Token de Design, as maravilhas das variáveis de semânticas absolutas como Color.Primary e porque o CSS de um site maduro se parece mais com uma árvore genealógica complexa do que uma página de arte pincelada na escura emoção matutina de um único artesão sonolento.</p>";
                return @"
<p>Aqui o ringue pega fogo. O inferno e o milagre do momento crucial o qual nomeamos de 'Handoff'. A entrega da arte estática feita no amado Figma pelo UI Designer para o assustado e metódico do Desenvolvedor Front-end.</p>
<p>Como alinhar as expectativas sobre limites reais do navegador, como não jogar toda a usabilidade pelo ralo com pesadas margens não estruturais e como chegar harmoniosamente na perfeição que só grandes corporações milionárias conseguem de primeiro lance e instinto polido.</p>";
            }

            return "<p>Conteúdo acadêmico riquíssimo recheado de lições de ouro para todos os amantes deste tópico. Aproveite cada minuto com pura garra para finalizar os estudos!</p>";
        }
    }
}

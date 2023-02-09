using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sons : MonoBehaviour
{
    public static Sons instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("Variaveis de Som")]
    [Header("Trilhas")]

    public AudioClip MusicaTensão1;
    public AudioClip MusicaTensão2;
    public AudioClip MusicaTensão3;

    public AudioClip MusicaDesespero1;
    public AudioClip MusicaDesespero2;

    public AudioClip MusicaPreenchimento1;
    public AudioClip MusicaPreenchimento2;
    public AudioClip MusicaPreenchimento3;

    public AudioClip MusicaMenu;

//-----------------Passos---------------------------------------------------------------------------------------
    [Header("Passos")]

    [Header("      Zelador")]

    public AudioClip ZeladorPisoconcretoAndando;
    public AudioClip ZeladorPisoconcretoCorrendo;
    public AudioClip ZeladorPisomadeiraAndando;
    public AudioClip ZeladorPisomadeiraCorrendo;

    [Header("      Protagonista")]

    public AudioClip ProtagonistaPisoconcretoAndando;
    public AudioClip ProtagonistaPisoconcretoCorrendo;
    public AudioClip ProtagonistaPisomadeiraAndando;
    public AudioClip ProtagonistaPisomadeiraCorrendo;

//-----------------Dublagem-------------------------------------------------------------------------------------
    [Header("Dublagem")]

    [Header("       Protagonista")]

    public AudioClip ProtagonistaRespiracaoNormal;
    public AudioClip ProtagonistaRespiracaoCorrendo;
    public AudioClip ProtagonistaRespiracaoSoltaAr;
    public AudioClip ProtagonistaRespiracaoTrancaAr;

    [Header("      Fitas")]

    public AudioClip Fita1;
    public AudioClip Fita2; 
    public AudioClip Fita3;
    public AudioClip Fita4;
    public AudioClip Fita5;

    [Header("      Zelador")]

    public AudioClip ZeladorGrunhidosNormal;
    public AudioClip ZeladorGrunhidosCorrendo;
    public AudioClip ZeladorGrunhidosGolpedepá;

    //-----------------Efeitos--------------------------------------------------------------------------------------------
    [Header("Props")]

    [Header("      Interação")]
    public AudioClip PortaEletronicaAbrindo;
    public AudioClip PortaEletronicaFechando;
    public AudioClip PortaNormalTrancada;
    public AudioClip PortaNormalAbrindolentamente;
    public AudioClip PortaCortafogoAbrindo;
    public AudioClip PortaCortafogoFechando;
    public AudioClip PortaCortafogoLentamente;
    public AudioClip PortaCortafogoBatendoforte;
    public AudioClip PortaCortafogoTrancada;
    public AudioClip PortaVidroAbrindo;
    public AudioClip PortaVidroFechando;
    public AudioClip PortaVidroLentamente;
    public AudioClip PortaVidroTrancada;
    public AudioClip PortaNormalFechando;
    public AudioClip PortaNormalAbrindo;
    
    
    [Header("               --")]
    public AudioClip LanternaLigar;
    public AudioClip LanternaGirarmanivela;
    public AudioClip LanternaArrastando;
    public AudioClip LanternaBateriaAcaba;
    [Header("               --")]
    public AudioClip IsqueiroAceso;
    public AudioClip IsqueiroAcender;
    public AudioClip IsqueiroGuardar;
    [Header("               --")]
    public AudioClip ChaveDestrancar;
    public AudioClip ChavePegar;
    [Header("               --")]
    public AudioClip GavetaFechar;
    public AudioClip GavetaAbrir;
    [Header("               --")]
    public AudioClip DutoPassos;
    public AudioClip DutoTampaCai;
    public AudioClip DutoTampaArrancada;
    [Header("               --")]
    public AudioClip InventarioAbrir;
    public AudioClip InventarioSelecionaritem;
    [Header("               --")]
    public AudioClip PapelAbrir;
    [Header("               --")]
    public AudioClip IdentificadordesenhaBotao;
    public AudioClip IdentificadordesenhaSenhaincorreta;
    public AudioClip IdentificadordesenhaSenhacorreta;
    [Header("               --")]
    public AudioClip ArmarioEntrar;
    public AudioClip ArmarioSair;
    [Header("               --")]
    public AudioClip ElevadorBotao;
    public AudioClip ElevadorAbre;
    public AudioClip ElevadorFecha;
    public AudioClip ElevadorInicio;
    public AudioClip ElevadorPara;
    public AudioClip ElevadorMovimentaçao;
    public AudioClip ElevadorVoltaenergia;
    public AudioClip ElevadorCaindoUnico;
    [Header("               --")]
    public AudioClip TvChiado;
    public AudioClip TvBotao;
    [Header("               --")]
    public AudioClip GeradorLigado;
    public AudioClip GeradorLigar;
    [Header("               --")]
    public AudioClip VidroquebrandoLeve;
    public AudioClip VidroquebrandoCabine;
    public AudioClip VidroquebrandoPorta;

    [Header("      Ambiente")]

    public AudioClip Metalarrastando;
    [Header("               --")]
    public AudioClip LampadaInterruptor;
    public AudioClip LampadaCaindo;
    public AudioClip LampadaAcender;
    public AudioClip LampadaApagar;
    public AudioClip LampadaFalhando;
    public AudioClip LampadaFalhandorapido;
    [Header("               --")]
    public AudioClip Chaodesabando;
    [Header("               --")]
    public AudioClip Alarme;
    [Header("               --")]
    public AudioClip ManequimCaicompleto;
    [Header("               --")]
    public AudioClip ObjetosinflamaveisAtearfogo;
    public AudioClip ObjetosinflamaveisPilhadepapelao;
    public AudioClip ObjetosinflamaveisLixeira;
    public AudioClip ObjetosinflamaveisPilhadejornal;
    [Header("               --")]
    public AudioClip PaVarrendo;   
    public AudioClip PaArrastandocorrendo;
    public AudioClip PaArrastandoandando;
    public AudioClip PaDebatendoelevador;
    public AudioClip PaBatendoelevador;
    public AudioClip PaBatendojogador;
    public AudioClip PaCaindo;
    [Header("               --")]
    public AudioClip ZeladorBatendoporta;

    [Header("Efeitos")]
    public AudioClip EfeitoDramatico1;
    public AudioClip EfeitoDramatico2;
    public AudioClip EfeitoDramatico3;
    public AudioClip EfeitoDramatico4;
    public AudioClip EfeitoDramatico5;
    //public AudioClip EfeitoDramatico6;
    //public AudioClip EfeitoDramatico7;
    //public AudioClip EfeitoDramatico8;
    //public AudioClip EfeitoDramatico9;

    void Start () {
		
	}
	

	void Update () {
		
	}
}

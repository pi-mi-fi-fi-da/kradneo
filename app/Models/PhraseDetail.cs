namespace app.Models;

public class PhraseDetail
{ 
    public PhraseDetail(Phrase phrase, List<PhraseProduct> phraseProducts)
    {
        this.phrase = phrase;
        this.phraseProducts = phraseProducts;
    }

    public Phrase phrase { get; set; }

    public List<PhraseProduct> phraseProducts { get; set; }

}

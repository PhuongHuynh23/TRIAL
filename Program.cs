using System;
using System.Collections.Generic;
using System.Linq;

// ---------------------------------------------- Define Card class --------------------------------------------------
public class Card
{
    public string Suit { get; private set; }
    public string Rank { get; private set; }

    public Card(string suit, string rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}

// ------------------------------------------------- Define Deck class -----------------------------------------------
public class Deck
{
    private List<Card> cards;
    private static readonly string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
    private static readonly string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
    private Random random = new Random();

    public Deck()
    {
        cards = new List<Card>();
        foreach (var suit in suits)
        {
            foreach (var rank in ranks)
            {
                cards.Add(new Card(suit, rank));
            }
        }
        Shuffle();
    }

    public void Shuffle()
    {
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            Card value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
    }

    public Card DealCard()
    {
        if (cards.Count > 0)
        {
            var card = cards[^1];
            cards.RemoveAt(cards.Count - 1);
            return card;
        }
        return null;
    }
}

// ----------------------------------------------- Define Player class -----------------------------------------------
public class Player
{
    public string Name { get; private set; }
    public List<Card> Hand { get; private set; }
    public int Chips { get; private set; }
    public int CurrentBet { get; private set; }
    public bool InGame { get; private set; }
    public bool IsAI { get; private set; }

    public Player(string name, bool isAI = false)
    {
        Name = name;
        Hand = new List<Card>();
        Chips = 100;
        CurrentBet = 0;
        InGame = true;
        IsAI = isAI;
    }

    public void ReceiveCard(Card card)
    {
        Hand.Add(card);
    }

    public string ShowHand()
    {
        return string.Join(", ", Hand);
    }

    public void Bet(int amount)
    {
        if (amount > Chips)
            throw new Exception("Not enough chips to bet that amount.");

        Chips -= amount;
        CurrentBet += amount;
    }

    public void Fold()
    {
        InGame = false;
    }

    public void ResetBet()
    {
        CurrentBet = 0;
    }
}

// --------------------------------------------- Define PokerGame class -----------------------------------------------
public class PokerGame
{
    private Deck deck;
    private List<Player> players;
    private List<Card> communityCards;
    private int pot;

    public PokerGame(List<Player> players)
    {
        deck = new Deck();
        this.players = players;
        communityCards = new List<Card>();
        pot = 0;
    }

    public void DealHands()
    {
        for (int i = 0; i < 2; i++)
        {
            foreach (var player in players)
            {
                player.ReceiveCard(deck.DealCard());
            }
        }
    }

    public void DealCommunityCards(int num)
    {
        for (int i = 0; i < num; i++)
        {
            communityCards.Add(deck.DealCard());
        }
    }

    public string ShowCommunityCards()
    {
        return string.Join(", ", communityCards);
    }

    public int AiDecision(Player player, int highestBet)
    {
        if (!player.InGame) return highestBet;

        Console.WriteLine($"{player.Name}'s (AI) turn. Current chips: {player.Chips}");
        string action;
        if (highestBet == 0 || new Random().Next(2) == 0)
        {
            action = "call";
        }
        else
        {
            action = "raise";
        }

        switch (action)
        {
            case "call":
                int betAmount = highestBet - player.CurrentBet;
                player.Bet(betAmount);
                pot += betAmount;
                Console.WriteLine($"{player.Name} calls with {betAmount} chips.");
                break;
            case "raise":
                int raiseAmount = new Random().Next(1, player.Chips);
                int totalBet = highestBet + raiseAmount;
                player.Bet(totalBet - player.CurrentBet);
                pot += totalBet - player.CurrentBet;
                Console.WriteLine($"{player.Name} raises to {totalBet} chips.");
                highestBet = totalBet;
                break;
        }

        return highestBet;
    }

    public int PlayerDecision(Player player, int highestBet)
    {
        if (!player.InGame) return highestBet;

        Console.WriteLine($"{player.Name}'s turn. Current chips: {player.Chips}");
        Console.WriteLine($"{player.Name}, do you want to fold, call ({highestBet}), or raise?");
        string action = Console.ReadLine().ToLower();

        switch (action)
        {
            case "fold":
                player.Fold();
                Console.WriteLine($"{player.Name} folds.");
                break;
            case "call":
                int betAmount = highestBet - player.CurrentBet;
                player.Bet(betAmount);
                pot += betAmount;
                Console.WriteLine($"{player.Name} calls with {betAmount} chips.");
                break;
            case "raise":
                Console.WriteLine("Enter the raise amount: ");
                int raiseAmount = int.Parse(Console.ReadLine());
                int totalBet = highestBet + raiseAmount;
                player.Bet(totalBet - player.CurrentBet);
                pot += totalBet - player.CurrentBet;
                Console.WriteLine($"{player.Name} raises to {totalBet} chips.");
                highestBet = totalBet;
                break;
            default:
                player.Fold();
                Console.WriteLine("Invalid action. Folding by default.");
                break;
        }

        return highestBet;
    }

    public void BettingRound()
    {
        Console.WriteLine("\nStarting a new betting round....");
        int highestBet = 0;

        foreach (var player in players)
        {
            highestBet = player.IsAI ? AiDecision(player, highestBet) : PlayerDecision(player, highestBet);
        }

        foreach (var player in players)
        {
            player.ResetBet();
        }
    }

    public void EvaluateHands()
    {
        int winner = 0;
        foreach (var player in players)
        {
            if (player.InGame)
            {
                int handStrength = EvaluateHandStrength(player.Hand);
                Console.WriteLine($"{player.Name}'s hand strength: {handStrength}");
            }
        }
        foreach (var player in players)
        {
            if (player.InGame)
            {
                int handStrength = EvaluateHandStrength(player.Hand);
                if (handStrength > winner)
                {
                    winner = handStrength;
                }

            }
        }
        foreach (var player in players)
        {
            if (player.InGame)
            {
                int handStrength = EvaluateHandStrength(player.Hand);
                if(handStrength == winner)
                {
                    Console.WriteLine($"The winner is {player.Name}");

                }
            }
        }
    }
    public int EvaluateHandStrength(List<Card> hand)
    {
        Dictionary<string, int> rankCount = new Dictionary<string, int>();

        // Count the occurrence of each rank
        foreach (var card in hand)
        {
            if (rankCount.ContainsKey(card.Rank))
                rankCount[card.Rank]++;
            else
                rankCount[card.Rank] = 1;
        }

        // Check for pairs, three of a kind, etc.
        if (rankCount.ContainsValue(3))
        {
            return 3;  // Three of a kind
        }
        else if (rankCount.ContainsValue(2))
        {
            return 2;  // Pair
        }

        // Return 1 for high card
        return 1;
    }

    public void Play()
    {
        // Initial deal
        DealHands();

        // Show players' hands
        foreach (var player in players)
        {
            Console.WriteLine($"{player.Name}'s hand: {player.ShowHand()}");
        }

        // First betting round 
        BettingRound();

        // Deal community cards 
        Console.WriteLine("\nDealing the Flop...");
        DealCommunityCards(3);
        Console.WriteLine($"Community Cards: {ShowCommunityCards()}");

        // Second betting round
        BettingRound();

        Console.WriteLine("\nDealing the Turn...");
        DealCommunityCards(1);
        Console.WriteLine($"Community Cards: {ShowCommunityCards()}");

        // Third betting round 
        BettingRound();

        Console.WriteLine("\nDealing the River...");
        DealCommunityCards(1);
        Console.WriteLine($"Community Cards: {ShowCommunityCards()}");

        // Final betting round
        BettingRound();

        // Showdown (simplified)
        foreach (var player in players)
        {
            if (player.InGame)
            {
                Console.WriteLine($"{player.Name}'s final hand: {player.ShowHand()}");
            }
        }
        Console.WriteLine($"Total pot: {pot} chips");

        Console.WriteLine("\nEvaluating hands...");
        EvaluateHands();
    }
}

// Main program to initiate a game
public class Program
{
    public static void Main()
    {
        var players = new List<Player>
        {
            new Player("Alice"),
            new Player("Cat"),
            new Player("Dog"),
            new Player("Bob (AI)", true)
            
        };

        PokerGame game = new PokerGame(players);
        game.Play();
    }
}
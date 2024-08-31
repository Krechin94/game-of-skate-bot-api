using GameOfSkateBotApi.GameLogic.Entity;

namespace GameOfSkateBotApi.GameLogic
{
	public class Tricks
	{
		//TODO: change to database storage  _context
		private List<Trick> _trickStorage = [];

		public Tricks()
		{
			GenerateTricks();
		}

		public IEnumerable<string> GenerateFor(Difficulty difficulty)
			=> difficulty switch
			{
				Difficulty.Easy => _trickStorage.Where(t => t.Difficulty <= Difficulty.Easy).Select(t => t.TrickName),
				Difficulty.Advanced => _trickStorage.Where(t => t.Difficulty <= Difficulty.Advanced).Select(t => t.TrickName),
				Difficulty.Hard => _trickStorage.Where(t => t.Difficulty <= Difficulty.Hard).Select(t => t.TrickName),
				Difficulty.OnlyAdvanced => _trickStorage.Where(t => t.Difficulty == Difficulty.Advanced).Select(t =>t.TrickName),
				Difficulty.OnlyPro => _trickStorage.Where(t => t.Difficulty == Difficulty.Hard).Select(t => t.TrickName),
				_ => throw new NotImplementedException($"Something went wrong - no such difficulty {(int)difficulty}")
			};

		#region Tricks Lists

		private void GenerateTricks()
		{
			foreach (var difficulty in Enum.GetValues<Difficulty>())
				_trickStorage.AddRange(
					difficulty switch
					{
						Difficulty.Easy => _easyTricks.Select(t => new Trick(t, difficulty)),
						Difficulty.Advanced => _advancedTricks.Select(t => new Trick(t, difficulty)),
                        Difficulty.Hard => _hardTricks.Select(t => new Trick(t, difficulty)),
                        Difficulty.OnlyAdvanced => _advancedTricks.Select(t => new Trick(t, difficulty)),
                        Difficulty.OnlyPro => _advancedTricks.Select(t => new Trick(t, difficulty)),
                        _ => throw new NotImplementedException($"Something went wrong - no such difficulty {(int)difficulty}")
					});
		}

		private static List<string> _easyTricks =
		[
			"ollie",
			"fakie ollie",
			"switch ollie",
			"nollie",
            /*"bs 180",
			"fakie bs 180",
			"switch bs 180",
			"nollie bs 180",
			"fs 180",
			"fakie fs 180",
			"switch fs 180",
			"nollie fs 180",
			"pop shove-it",
			"fakie pop shove-it",
			"switch pop shove-it",
			"nollie pop shove-it",
			"fs pop shove-it",
			"fakie fs pop shove-it",
			"switch fs pop shove-it",
			"nollie fs pop shove-it",
			"fakie bs big spin",
			"fakie 360 pop shove-it",
			"kickflip",
			"fakie kickflip",
			"heelflip",
			"fakie heelflip"*/
        ];
		private static List<string> _advancedTricks =
		[
			"varial kickflip",
			"fakie varial kickflip",
			"varial heelflip",
			"fakie varial heelflip",
            /*"bs 180 kickflip",
			"fakie halfcab kickflip",
			"fs 180 kickflip",
			"fakie bs 180 kickflip",
			"bs 180 heelflip",
			"fakie halfcab heelflip",
			"fs 180 heelflip",
			"fakie bs 180 heelflip",
			"360 pop shove-it",
			"nollie 360 pop shove-it",
			"big spin",
			"nollie big spin",
			"nollie fs big spin",
			"fakie fs big spin",
			"harflip",
			"fakie hardflip",
			"360 flip",
			"fakie 360 flip",
			"big flip",
			"fakie big flip",
			"nollie 360 fs shove it",
			"inward heelflip",
			"fakie inward heelflip"*/
        ];
		private static List<string> _hardTricks =
		[
			"switch kickflip",
			"nollie kickflip",
			"switch heelflip",
			"nollie heelflip",
			"switch 360 flip",
			"nollie 360 flip",
			"switch bs 180 kickflip",
			"nollie bs 180 kickflip",
			"switch fs 180 kickflip",
			"nollie fs 180 kickflip",
			"switch bs 180 heelflip",
			"nollie bs 180 heelflip",
			"switch fs 180 heelflip",
			"nollie fs 180 heelflip",
			"switch varial heelflip",
			"nollie varial heelflip",
			"switch varial kickflip",
			"nollie varial kickflip",
		];
		#endregion
	}
}

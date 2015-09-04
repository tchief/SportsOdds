using ReactiveUI;
using System.Windows.Media;

namespace SportsOdds
{
    public class OddsViewModel : ReactiveObject
    {
        private readonly Brush _homeColor = Brushes.DarkGreen;
        private readonly Brush _guestColor = Brushes.Black;

        public OddsViewModel(Odds odd)
        {
            Model = odd;
        }

        public Odds Model { get; private set; }

        public Brush FavoriteColor { get { return Model.FavoriteTeamName == Model.HomeTeamName ? _homeColor : _guestColor; } }
        public Brush UnderdogColor { get { return Model.UnderdogTeamName == Model.HomeTeamName ? _homeColor : _guestColor; } }
    }
}

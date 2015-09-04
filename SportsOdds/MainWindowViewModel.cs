using ReactiveUI;
using System.Collections.ObjectModel;

namespace SportsOdds
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            Odds = new ObservableCollection<OddsViewModel>();
            foreach (var o in SportsOdds.GetOdds())
                Odds.Add(new OddsViewModel(o));
        }

        public ObservableCollection<OddsViewModel> Odds { get; private set; }
    }
}

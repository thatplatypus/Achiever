import SwiftUI
struct ContentView: View {
    @EnvironmentObject var userSettings: UserSettings

       var body: some View {
           NavigationView {
               VStack {
                   if userSettings.userEmail == "guest" {
                       LoginView()
                   } else {
                       GoalOverviewView()
                   }
               }
           }
       }
}

class UserSettings: ObservableObject {
    @Published var userEmail: String = "guest"
}

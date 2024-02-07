import SwiftUI
struct ContentView: View {
    @State private var userEmail = "guest"

    var body: some View {
        NavigationView {
            VStack {
                if userEmail == "guest" {
                    LoginView(userEmail: $userEmail)
                } else {
                    GoalOverviewView()
                }
            }
        }
    }
    
    func logout() {
        userEmail = "guest"
    }
}

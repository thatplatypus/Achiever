//
//  GoalOverviewView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/1/24.
//

import Foundation
import SwiftUI

struct GoalOverviewView: View {
    @StateObject var goalData = GoalData()

    var body: some View {
        VStack {
            if goalData.isLoading {
                ProgressView("Loading...")
            } else {
                ScrollView {
                    LazyVGrid(columns: [GridItem(.adaptive(minimum: 200))]) {
                        ForEach(goalData.goals) { goal in
                            GoalCardView(goal: goal)
                                .padding(.horizontal, 16)
                        }
                    }
                }
            }
        }
        .navigationBarTitle("Goals", displayMode: .inline)
        .navigationBarItems(trailing: NavigationLink(destination: SettingsView()) {
            Image(systemName: "gearshape")
        })
    }
}
    

//
//  GoalOverviewView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/1/24.
//

import Foundation
import SwiftUI

struct GoalOverviewView: View {
    @State private var showingPopover = false
    @State private var showingFilter = false
    @State private var searchText = ""
    @ObservedObject var goalData = GoalData()
    @AppStorage("isDarkMode") var isDarkMode: Bool = false
    let goalClient = GoalClient(networkManager: NetworkManager())
    
    var body: some View {
        VStack {
            if goalData.isLoading {
                LoadingView()
                    .preferredColorScheme(isDarkMode ? .dark : .light)
                
            } else {
                VStack {
                    ScrollView {
                        LazyVGrid(columns: [GridItem(.adaptive(minimum: 200))]) {
                            ForEach(goalData.filteredGoals) { goal in
                                GoalCardView(goal: goal){ updatedGoal in
                                    if let index = goalData.goals.firstIndex(where: { $0.id == updatedGoal.id }) {
                                            goalData.goals[index] = updatedGoal
                                    }
                                }
                                 .padding(.horizontal, 16)
                            }
                        }
                    }
                    .refreshable {
                        goalData.loadGoals(refresh: true)
                    }
                }
                .searchable(text: $goalData.searchText)
                .preferredColorScheme(isDarkMode ? .dark : .light)
            }
        }
        .navigationBarTitle("Goals", displayMode: .inline)
        .navigationBarItems(
            leading:
                Button(action: { showingFilter = true }) {
                    Image(systemName: "line.horizontal.3.decrease")
                }
                .popover(isPresented: $showingFilter) {
                    FilterView(sortOption: $goalData.sortOption, hideCompletedGoals: $goalData.hideCompletedGoals, hidePastGoals: $goalData.hidePastGoals)
                },
            trailing:
                HStack {
                    Button(action: { showingPopover = true }) {
                        Image(systemName: "plus")
                    }
                    .popover(isPresented: $showingPopover) {
                        AddNewGoalView(onSave: { newGoal in
                            print(newGoal)
                            goalClient.createGoal(goal: newGoal, completion: { result in
                                switch result {
                                case .success(let successGoal):
                                    print("Goal created, id: \(successGoal)")
                                    goalClient.getGoalById(id: successGoal, completion: { fetch in
                                        switch fetch {
                                        case .success(let g):
                                            goalData.goals.append(g)
                                        case .failure(let f):
                                            print("Failed to retrieve goal: \(f)")
                                        }
                                    })
                                case .failure(let error):
                                    print("Failed to create goal: \(error)")
                                }
                            })
                            self.showingPopover = false
                        })
                    }
                    NavigationLink(destination: SettingsView()) {
                        Image(systemName: "gearshape")
                    }
                }
        )
    }
}
    

    

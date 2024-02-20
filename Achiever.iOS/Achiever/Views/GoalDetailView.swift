//
//  GoalDetailView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/2/24.
//

import Foundation
import SwiftUI

struct GoalDetailView: View {
    @State var goal: Goal
    @State private var newSubTasks: [SubTask]
    @State private var inProgressSubTasks: [SubTask]
    @State private var completedSubTasks: [SubTask]
    @State private var showingAddSubtaskModal = false


    init(goal: Goal) {
        _goal = State(initialValue: goal)
        let subTasks = goal.subTasks ?? []
        _newSubTasks = State(initialValue: subTasks.filter { $0.status?.lowercased() == "new" })
        _inProgressSubTasks = State(initialValue: subTasks.filter { $0.status?.lowercased() == "inprogress" })
        _completedSubTasks = State(initialValue: subTasks.filter { $0.status?.lowercased() == "completed" })

    }

    var body: some View {
        List {
            Section(header: Text("New")) {
                if newSubTasks.isEmpty {
                    Text("No tasks")
                } else {
                    ForEach(Array(newSubTasks.enumerated()), id: \.element.id) { index, _ in
                        SubTaskCard(subTask: $newSubTasks[index], goal: $goal, color: .gray)
                    }
                }
            }

            Section(header: Text("In Progress")) {
                if inProgressSubTasks.isEmpty {
                    Text("No tasks")
                } else {
                    ForEach(Array(inProgressSubTasks.enumerated()), id: \.element.id) { index, _ in
                        SubTaskCard(subTask: $inProgressSubTasks[index], goal: $goal, color: .cyan)
                    }
                }
            }
        
    
            
            Section(header: Text("Completed")) {
                if completedSubTasks.isEmpty {
                    Text("No tasks")
                } else {
                    ForEach(Array(completedSubTasks.enumerated()), id: \.element.id) { index, _ in
                        SubTaskCard(subTask: $completedSubTasks[index], goal: $goal, color: .green)
                    }
                }
            }
        }
        .navigationTitle(goal.title ?? "")
                .toolbar {
                    Button(action: { showingAddSubtaskModal = true }) {
                        Image(systemName: "plus")
                    }
                }
                .sheet(isPresented: $showingAddSubtaskModal) {
                    SubtaskDetailModal(goal: $goal, subTask: .constant(SubTask()), onSave: { updatedGoal in
                        print(updatedGoal)
                        goal = updatedGoal
                        let newSubTask = goal.subTasks!.last!
                        switch newSubTask.status?.lowercased() {
                        case "new":
                            newSubTasks.append(newSubTask)
                        case "inprogress":
                            inProgressSubTasks.append(newSubTask)
                        case "completed":
                            completedSubTasks.append(newSubTask)
                        default:
                            break
                        }
                    })
                }
    }
}

struct SubTaskCard: View {
    @Binding var subTask: SubTask
    @Binding var goal: Goal
    @State private var showingDetail = false
    var color: Color

    var body: some View {
        HStack {
            Rectangle()
                .fill(color)
                .frame(width: 10)
                .cornerRadius(10)
            Text(subTask.title ?? "")
        }
        .frame(maxWidth: .infinity, alignment: .leading)
        .padding()
        .background(Color(UIColor.systemBackground))
        .cornerRadius(10)
        .shadow(radius: 5)
        .onTapGesture {
            showingDetail = true
        }
        .sheet(isPresented: $showingDetail) {
            SubtaskDetailModal(goal: $goal, subTask: $subTask, onSave: { updatedGoal in
                print(updatedGoal)
                goal = updatedGoal
            })
            }
        }
    }


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
    var goalUpdated: (Goal) -> Void


    init(goal: Goal, onGoalUpdated: @escaping (Goal) -> Void = { _ in }) {
        _goal = State(initialValue: goal)
        let subTasks = goal.subTasks ?? []
        _newSubTasks = State(initialValue: subTasks.filter { $0.status?.lowercased() == "new" })
        _inProgressSubTasks = State(initialValue: subTasks.filter { $0.status?.lowercased() == "inprogress" })
        _completedSubTasks = State(initialValue: subTasks.filter { $0.status?.lowercased() == "completed" })
        self.goalUpdated = onGoalUpdated
    }

    var body: some View {
        List {
            Section(header: Text("New")) {
                if newSubTasks.isEmpty {
                    Text("No tasks")
                } else {
                    ForEach(Array(newSubTasks.enumerated()), id: \.element.id) { index, _ in
                        SubTaskCard(subTask: $newSubTasks[index], goal: $goal, color: .gray, subtaskUpdated: { goal in
                            goalUpdated(goal)
                            newSubTasks = goal.subTasks?.filter { $0.status?.lowercased() == "new" && $0.id != UUID(uuidString: "00000000-0000-0000-0000-000000000000") } ?? []
                            inProgressSubTasks = goal.subTasks?.filter { $0.status?.lowercased() == "inprogress" && $0.id != UUID(uuidString: "00000000-0000-0000-0000-000000000000")} ?? []
                            completedSubTasks = goal.subTasks?.filter { $0.status?.lowercased() == "completed" && $0.id != UUID(uuidString: "00000000-0000-0000-0000-000000000000")} ?? []

                        }, subtaskDeleted: {subtaskId in
                                goal.subTasks?.removeAll(where: {$0.id == subtaskId})
                                goalUpdated(goal)
                                newSubTasks.removeAll(where: {$0.id == subtaskId})
                            })
                    }
                }
            }

            Section(header: Text("In Progress")) {
                if inProgressSubTasks.isEmpty {
                    Text("No tasks")
                } else {
                    ForEach(Array(inProgressSubTasks.enumerated()), id: \.element.id) { index, _ in
                        SubTaskCard(subTask: $inProgressSubTasks[index], goal: $goal, color: .cyan, subtaskUpdated: {goal in
                            newSubTasks = goal.subTasks?.filter { $0.status?.lowercased() == "new" && $0.id != UUID(uuidString: "00000000-0000-0000-0000-000000000000") } ?? []
                            inProgressSubTasks = goal.subTasks?.filter { $0.status?.lowercased() == "inprogress" && $0.id != UUID(uuidString: "00000000-0000-0000-0000-000000000000")} ?? []
                            completedSubTasks = goal.subTasks?.filter { $0.status?.lowercased() == "completed" && $0.id != UUID(uuidString: "00000000-0000-0000-0000-000000000000")} ?? []
                            goalUpdated(goal)}, subtaskDeleted: {subtaskId in
                                goal.subTasks?.removeAll(where: {$0.id == subtaskId})
                                goalUpdated(goal)
                                inProgressSubTasks.removeAll(where: {$0.id == subtaskId})
                            })
                    }
                }
            }
        
            Section(header: Text("Completed")) {
                if completedSubTasks.isEmpty {
                    Text("No tasks")
                } else {
                    ForEach(Array(completedSubTasks.enumerated()), id: \.element.id) { index, _ in
                        SubTaskCard(subTask: $completedSubTasks[index], goal: $goal, color: .green, subtaskUpdated: {goal in
                            newSubTasks = goal.subTasks?.filter { $0.status?.lowercased() == "new" && $0.id != UUID(uuidString: "00000000-0000-0000-0000-000000000000") } ?? []
                            inProgressSubTasks = goal.subTasks?.filter { $0.status?.lowercased() == "inprogress" && $0.id != UUID(uuidString: "00000000-0000-0000-0000-000000000000")} ?? []
                            completedSubTasks = goal.subTasks?.filter { $0.status?.lowercased() == "completed" && $0.id != UUID(uuidString: "00000000-0000-0000-0000-000000000000")} ?? []

                            goalUpdated(goal)}, subtaskDeleted: {subtaskId in
                                goal.subTasks?.removeAll(where: {$0.id == subtaskId})
                                goalUpdated(goal)
                                completedSubTasks.removeAll(where: {$0.id == subtaskId})
                            })
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
                        goalUpdated(goal)
                        let newSubTask = goal.subTasks!.last!
                        switch newSubTask.status?.lowercased() {
                        case "new":
                            newSubTasks = (goal.subTasks?.filter({ $0.status?.lowercased() == "new" }))!
                        case "inprogress":
                            inProgressSubTasks = (goal.subTasks?.filter({ $0.status?.lowercased() == "inprogress" }))!
                        case "completed":
                            completedSubTasks = (goal.subTasks?.filter({ $0.status?.lowercased() == "completed" }))!
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
    var subtaskUpdated: (Goal) -> Void
    var subtaskDeleted: (UUID) -> Void

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
                subtaskUpdated(goal)
            },
            onDelete: { deletedSubtaskId in
                subtaskDeleted(deletedSubtaskId)
           })
        }
    }
}


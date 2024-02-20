//
//  SubtaskDetailModal.swift
//  Achiever
//
//  Created by Tom Brewer on 2/2/24.
//

import Foundation
import SwiftUI

struct SubtaskDetailModal: View {
    @Binding var goal: Goal
    @Binding var subTask: SubTask
    @State var showingDeleteAlert: Bool = false
    @State private var sliderValue: Double
    var onSave: ((Goal) -> Void) =  {_ in}
    @Environment(\.presentationMode) var presentationMode
    let goalClient = GoalClient(networkManager: NetworkManager())
    
    @State private var draftSubTask: EditableSubTask
    let statuses = ["New", "InProgress", "Completed"]
    let displayStatuses = ["New", "In Progress", "Completed"]
    
    init(goal: Binding<Goal>, subTask: Binding<SubTask>, onSave: @escaping (Goal) -> Void = { _ in })  {
        _goal = goal
        _subTask = subTask
        _draftSubTask = State(initialValue: EditableSubTask(
            title: subTask.wrappedValue.title ?? "New Task",
            status: subTask.wrappedValue.status ?? "New",
            estimatedHours: subTask.wrappedValue.estimatedHours ?? 0,
            note: subTask.wrappedValue.note ?? ""
        ))
        _sliderValue = State(initialValue: subTask.wrappedValue.estimatedHours ?? 0)
        self.onSave = onSave
    }
    
    func toDisplayStatus(status: String) -> String {
        if(status == "InProgress"){
            return "In Progress"
        }
            
        return status
    }
    
    var body: some View {
        NavigationView {
            VStack(alignment: .leading) {
                Form {
                    VStack(alignment: .leading) {
                        Text("Title")
                            .font(.footnote)
                            .foregroundColor(.gray)
                        TextField("Title", text: $draftSubTask.title)
                    }
                    
                    Picker("Status", selection: $draftSubTask.status) {
                        ForEach(statuses, id: \.self) {
                            Text(toDisplayStatus(status: $0)).tag($0)
                        }
                    }
                    
                    VStack(alignment: .leading) {
                        Text("Estimated Hours")
                            .font(.footnote)
                            .foregroundColor(.gray)
                        
                        HStack {
                            Image(systemName: "clock")
                                .resizable()
                                .foregroundColor(Color.accentColor)
                                .frame(width: 20, height: 20)
                            Text("\(sliderValue, specifier: "%.1f")")
                            Slider(value: $sliderValue, in: 0...12, step: 0.5)
                                .padding(4)
                            Spacer()
                            Button(action: {
                                if sliderValue > 0 {
                                    sliderValue -= 0.5
                                }
                            }) {
                                Image(systemName: "minus.circle")
                                    .resizable()
                                    .frame(width: 20, height: 20)
                            }
                            .buttonStyle(.borderless)
                            Button(action: {
                                if sliderValue < 12 {
                                    sliderValue += 0.5
                                }
                            }) {
                                Image(systemName: "plus.circle")
                                    .resizable()
                                    .frame(width: 20, height: 20)
                            }
                            .buttonStyle(.borderless)
                        }
                        
                        HStack {
                            Spacer()
                            ForEach([0.5, 1, 2, 4, 8], id: \.self) { value in
                                let presetValue = value
                                Button(action: { sliderValue = presetValue }) {
                                    Text("\(presetValue, specifier: "%.2g")")
                                        .foregroundColor(.white)
                                        .frame(width: 35, height: 2)
                                        .font(.footnote)
                                        .padding(10)
                                        .background(Color.accentColor)
                                        .cornerRadius(10)
                                }
                                .buttonStyle(.borderless)
                            }
                            Spacer()
                        }
                    }
                    VStack(alignment: .leading) {
                        HStack {
                            Image(systemName: "note.text")
                                .resizable()
                                .foregroundColor(Color.accentColor)
                                .frame(width: 20, height: 20)
                            Text("Note")
                                .font(.footnote)
                                .foregroundColor(.gray)
                        }
                        TextEditor(text: $draftSubTask.note)
                            .frame(minHeight:200)
                    }
                    Button(action: {
                        showingDeleteAlert = true
                    }) {
                        HStack {
                            Spacer()
                            Text("Delete")
                                .foregroundColor(.white)
                            Image(systemName: "trash")
                                .foregroundColor(.white)
                            Spacer()
                        }
                    }
                    .padding(16)
                    .background(Color.red)
                    .cornerRadius(10)
                    .alert(isPresented: $showingDeleteAlert) {
                        Alert(title: Text("Are you sure you want to delete this task?"),
                              primaryButton: .destructive(Text("Delete")) {
                            // Implement your delete logic here
                        },
                              secondaryButton: .cancel())
                    }
                }
            }
            .navigationTitle(subTask.id != UUID(uuidString: "00000000-0000-0000-0000-000000000000") ? "Edit Task" : "Add Task")
            .toolbar {
                ToolbarItem(placement: .navigationBarLeading) {
                    Button("Cancel") {
                        presentationMode.wrappedValue.dismiss()
                    }
                }
                ToolbarItem(placement: .navigationBarTrailing) {
                    Button("Save") {
                        draftSubTask.estimatedHours = sliderValue
                        subTask.title = draftSubTask.title
                        subTask.status = draftSubTask.status
                        subTask.estimatedHours = draftSubTask.estimatedHours
                        subTask.note = draftSubTask.note

                        if let index = goal.subTasks?.firstIndex(where: { $0.id == subTask.id }) {
                            goal.subTasks?[index] = subTask
                        } else {
                            var s = SubTask()
                            s.title = draftSubTask.title
                            s.status = draftSubTask.status
                            s.estimatedHours = draftSubTask.estimatedHours
                            s.note = draftSubTask.note
                            goal.subTasks?.append(s)
                        }

                        goalClient.updateGoal(goal: goal){ result in
                            switch result {
                            case .success(let updatedGoal):
                                print("Goal updated: \(updatedGoal)")
                                onSave(goal)
                            case .failure(let error):
                                print("Failed to update goal: \(error)")
                            }

                            presentationMode.wrappedValue.dismiss()
                        }
                    }
                }
            }          
        }
    }
    
    
    struct EditableSubTask {
        var title: String
        var status: String
        var estimatedHours: Double
        var note: String
    }
}

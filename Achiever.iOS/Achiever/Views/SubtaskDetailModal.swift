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
    @Environment(\.presentationMode) var presentationMode

    @State private var draftSubTask: EditableSubTask
    let statuses = ["New", "InProgress", "Completed"]

        init(goal: Binding<Goal>, subTask: Binding<SubTask>) {
        _goal = goal
        _subTask = subTask
        _draftSubTask = State(initialValue: EditableSubTask(
            title: subTask.wrappedValue.title ?? "New Task",
            status: subTask.wrappedValue.status ?? "new",
            estimatedHours: subTask.wrappedValue.estimatedHours ?? 0,
            note: subTask.wrappedValue.note ?? ""
        ))
    }

    var body: some View {
        NavigationView {
            Form {
                HStack {
                    Label("Title", systemImage: "")
                        .fixedSize(horizontal: true, vertical: false)
                            .lineLimit(1)
                    TextField("Title", text: $draftSubTask.title)
                }

                Picker("Status", selection: $draftSubTask.status) {
                    ForEach(statuses, id: \.self) {
                        Text($0).tag($0)
                    }
                }

                    TextField("Estimated hours", value: $draftSubTask.estimatedHours, formatter: {
                        let formatter = NumberFormatter()
                        formatter.numberStyle = .decimal
                        formatter.minimumSignificantDigits = 2
                        return formatter
                    }())
                    

                HStack {
                    Label("Note", systemImage: "note.text")
                    TextEditor(text: $draftSubTask.note)
                        .frame(minHeight:100)
                }
                
                
                                
            }
            .navigationTitle("Edit")
            .toolbar {
                ToolbarItem(placement: .navigationBarLeading) {
                    Button("Cancel") {
                        presentationMode.wrappedValue.dismiss()
                    }
                }
                ToolbarItem(placement: .navigationBarTrailing) {
                    Button("Save") {
                        if let index = goal.subTasks?.firstIndex(where: { $0.id == subTask.id }) {
                            goal.subTasks?[index].title = draftSubTask.title
                            goal.subTasks?[index].status = draftSubTask.status
                            goal.subTasks?[index].estimatedHours = draftSubTask.estimatedHours
                            goal.subTasks?[index].note = draftSubTask.note
                            // Update goal
                        }
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

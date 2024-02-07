import Foundation
import SwiftUI
import UniformTypeIdentifiers

struct GoalDetailViewDragAndDrop: View {
    var goal: Goal
    @State private var subTasks: [SubTask]
    
    init(goal: Goal) {
        self.goal = goal
        _subTasks = State(initialValue: goal.subTasks ?? [])
    }
    
    var body: some View {
        VStack {
            ForEach(subTasks, id: \.id) { subTask in
                Text(subTask.title ?? "")
                    .draggable(subTask)
            }
        }
        .dropDestination(for: SubTask.self) { items, location in
            // Handle the dropped task
            guard let task = items.first else {
                return false
            }
            // Update the task in the Core Data context
            return true
        }
        .navigationTitle(goal.title ?? "")
        .toolbar {
            EditButton()
        }
    }
}

extension SubTask: Transferable {
    static var transferRepresentation: some TransferRepresentation {
        CodableRepresentation(contentType: .subTask)
    }
}

extension UTType {
    static var subTask: UTType { UTType(exportedAs: "com.example.subTask") }
}

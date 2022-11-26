#import <Foundation/Foundation.h>

@protocol NativeCallsProtocol
@required
- (void)receiveFinish;
// Other methods
@end

__attribute__ ((visibility("default")))
@interface FrameworkLibAPI : NSObject
+(void) registerAPIforNativeCalls:(id<NativeCallsProtocol>) aApi;

@end